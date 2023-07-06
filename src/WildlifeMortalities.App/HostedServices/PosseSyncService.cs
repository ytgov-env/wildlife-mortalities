using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Shared.Services.Posse;

namespace WildlifeMortalities.App.HostedServices;

public class PosseSyncService : TimerBasedHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public PosseSyncService(IServiceProvider serviceProvider)
        : base(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(2)) =>
        _serviceProvider = serviceProvider;

    protected override async Task DoWork(object? state)
    {
        Log.Information("Starting posse sync");
        using var scope = _serviceProvider.CreateScope();
        var posseService = scope.ServiceProvider.GetRequiredService<IPosseService>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var appConfiguration = scope.ServiceProvider.GetRequiredService<IAppConfigurationService>();

        var personMapper = context.People
            .AsSplitQuery()
            .OfType<PersonWithAuthorizations>()
            .Include(x => x.DraftReports)
            .Include(x => ((Client)x).SpecialGuideLicencesAsClient)
            .Include(x => ((Client)x).IndividualHuntedMortalityReports)
            .Include(x => ((Client)x).OutfitterGuidedHuntReportsAsClient)
            .Include(x => ((Client)x).SpecialGuidedHuntReportsAsGuide)
            .Include(x => ((Client)x).SpecialGuidedHuntReportsAsClient)
            .Include(x => ((Client)x).TrappedMortalitiesReports)
            .Include(x => x.Authorizations)
            .ThenInclude(x => x.Season)
            .Include(x => x.Authorizations)
            .ThenInclude(x => ((BigGameHuntingLicence)x).OutfitterAreas)
            .Include(x => x.Authorizations)
            .ThenInclude(x => ((SmallGameHuntingLicence)x).OutfitterAreas)
            .Include(x => x.Authorizations)
            .ThenInclude(x => ((OutfitterChiefGuideLicence)x).OutfitterAreas)
            .Include(x => x.Authorizations)
            .ThenInclude(x => ((OutfitterAssistantGuideLicence)x).OutfitterAreas)
            .Include(x => x.Authorizations)
            .ThenInclude(x => ((TrappingLicence)x).RegisteredTrappingConcession)
            .ToDictionary(x => x.EnvPersonId, x => x);

        var lastSuccessfulClientsSync = await appConfiguration.TryGetValue(
            Constants.AppConfigurationService.LastSuccessfulClientsSyncKey,
            new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.FromHours(-7))
        );
        var clientsSyncInitiatedTimestamp = await SyncClients(
            personMapper,
            context,
            posseService,
            lastSuccessfulClientsSync
        );
        await appConfiguration.SetValue(
            Constants.AppConfigurationService.LastSuccessfulClientsSyncKey,
            clientsSyncInitiatedTimestamp
        );

        var lastSuccessfulAuthorizationsSync = await appConfiguration.TryGetValue(
            Constants.AppConfigurationService.LastSuccessfulAuthorizationsSyncKey,
            new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.FromHours(-7))
        );
        var authorizationsSyncInitiatedTimestamp = await SyncAuthorizations(
            personMapper,
            context,
            posseService,
            lastSuccessfulAuthorizationsSync
        );
        await appConfiguration.SetValue(
            Constants.AppConfigurationService.LastSuccessfulAuthorizationsSyncKey,
            authorizationsSyncInitiatedTimestamp
        );
        Log.Information("Finished posse sync");
    }

    private static async Task<DateTimeOffset> SyncClients(
        Dictionary<string, PersonWithAuthorizations> personMapper,
        AppDbContext context,
        IPosseService posseService,
        DateTimeOffset lastSuccessfulSync
    )
    {
        var syncInitiatedTimestamp = DateTimeOffset.Now;
        var clientsInResponse = await posseService.GetClients(lastSuccessfulSync.AddMinutes(-10));

        foreach (
            var (clientInResponse, previousEnvClientIds) in clientsInResponse.OrderBy(
                x => x.client.LastModifiedDateTime
            )
        )
        {
            // Determine which are existing clients, and update them
            if (personMapper.TryGetValue(clientInResponse.EnvPersonId, out var clientInDatabase))
            {
                if (clientInDatabase.LastModifiedDateTime != clientInResponse.LastModifiedDateTime)
                {
                    clientInDatabase.Update(clientInResponse);
                    await context.SaveChangesAsync();
                }
            }
            // Or, add a new client
            else
            {
                context.Add(clientInResponse);
                await context.SaveChangesAsync();
                personMapper.Add(clientInResponse.EnvPersonId, clientInResponse);
                clientInDatabase = clientInResponse;
            }

            // Determine if client was merged in POSSE, and merge them
            foreach (var envClientId in previousEnvClientIds)
            {
                if (!personMapper.TryGetValue(envClientId, out var clientToBeMerged))
                    continue;
                var wasMerged = clientInDatabase.Merge(clientToBeMerged);
                if (!wasMerged)
                    continue;
                context.People.Remove(clientToBeMerged);
                personMapper.Remove(clientToBeMerged.EnvPersonId);
                await context.SaveChangesAsync();
            }
        }
        return syncInitiatedTimestamp;
    }

    private static async Task<DateTimeOffset> SyncAuthorizations(
        Dictionary<string, PersonWithAuthorizations> personMapper,
        AppDbContext context,
        IPosseService posseService,
        DateTimeOffset lastSuccessfulSync
    )
    {
        var syncInitiatedTimestamp = DateTimeOffset.Now;
        var validAuthorizationsInResponse = await posseService.GetAuthorizations(
            lastSuccessfulSync.AddMinutes(-10),
            personMapper,
            context
        );

        // Todo: remove grouping once PosseId is implemented
        var authorizationsInDatabase = personMapper
            .SelectMany(x => x.Value.Authorizations ?? new List<Authorization>())
            .GroupBy(x => x.GetUniqueIdentifier())
            .ToDictionary(x => x.Key, x => x.Select(y => y));
        foreach (var authInResponse in validAuthorizationsInResponse)
        {
            var inputKey = authInResponse.GetUniqueIdentifier();
            if (authorizationsInDatabase.TryGetValue(inputKey, out var authsInDatabase))
            {
                foreach (var authInDatabase in authsInDatabase)
                {
                    if (authInDatabase.LastModifiedDateTime != authInResponse.LastModifiedDateTime)
                    {
                        authInDatabase.Update(authInResponse);
                    }
                }
            }
            else
            {
                context.Authorizations.Add(authInResponse);
            }
        }

        await context.SaveChangesAsync();
        return syncInitiatedTimestamp;
    }
}
