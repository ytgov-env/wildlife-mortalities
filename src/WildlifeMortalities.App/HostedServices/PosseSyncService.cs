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

    private const string LastSuccessfulClientsSyncKey =
        "PosseSyncService.LastSuccessfulClientsSync";
    private const string LastSuccessfulAuthorizationsSyncKey =
        "PosseSyncService.LastSuccessfulAuthorizationsSync";

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
            .Include(x => ((Client)x).OutfitterGuidedHuntReportsAsChiefGuide)
            .Include(x => ((Client)x).OutfitterGuidedHuntReportsAsAssistantGuide)
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
            LastSuccessfulClientsSyncKey,
            DateTimeOffset.MinValue
        );
        var clientsSyncInitiatedTimestamp = await SyncClients(
            personMapper,
            context,
            posseService,
            lastSuccessfulClientsSync
        );
        await appConfiguration.SetValue(
            LastSuccessfulClientsSyncKey,
            clientsSyncInitiatedTimestamp
        );

        var lastSuccessfulAuthorizationsSync = await appConfiguration.TryGetValue(
            LastSuccessfulAuthorizationsSyncKey,
            DateTimeOffset.MinValue
        );
        var authorizationsSyncInitiatedDateTime = await SyncAuthorizations(
            personMapper,
            context,
            posseService,
            lastSuccessfulAuthorizationsSync
        );
        await appConfiguration.SetValue(
            LastSuccessfulAuthorizationsSyncKey,
            authorizationsSyncInitiatedDateTime
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
        var recentlyModifiedClients = await posseService.GetClients(lastSuccessfulSync);

        foreach (
            var (client, previousEnvClientIds) in recentlyModifiedClients.OrderBy(
                x => x.client.LastModifiedDateTime
            )
        )
        {
            // Determine which are existing clients, and update them
            if (personMapper.TryGetValue(client.EnvPersonId, out var clientInDatabase))
            {
                clientInDatabase.Update(client);
                await context.SaveChangesAsync();
            }
            // Or, add a new client
            else
            {
                context.Add(client);
                await context.SaveChangesAsync();
                personMapper.Add(client.EnvPersonId, client);
                clientInDatabase = client;
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
        var validAuthorizations = await posseService.GetAuthorizations(
            lastSuccessfulSync,
            personMapper,
            context
        );

        var existingAuthorizations = personMapper
            .SelectMany(x => x.Value.Authorizations)
            .ToDictionary(GetUniqueIdentifier, x => x);
        foreach (var authorization in validAuthorizations)
        {
            var inputKey = GetUniqueIdentifier(authorization);
            existingAuthorizations.TryGetValue(inputKey, out var existingAuthorization);

            if (existingAuthorization != null)
            {
                existingAuthorization.Update(authorization);
            }
            else
            {
                context.Authorizations.Add(authorization);
            }
        }

        await context.SaveChangesAsync();
        return syncInitiatedTimestamp;

        static string GetNormalizedNumber(Authorization authorization) =>
            authorization.Number.EndsWith('C') ? authorization.Number[..^1] : authorization.Number;

        static string GetUniqueIdentifier(Authorization authorization)
        {
            var result =
                $"{GetNormalizedNumber(authorization)}-{authorization.GetType().Name}-{authorization.Person.Id}-{authorization.Season.Id}";
            return result;
        }
    }
}
