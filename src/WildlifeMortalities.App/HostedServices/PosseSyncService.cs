using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Shared.Services.Posse;

namespace WildlifeMortalities.App.HostedServices;

public class PosseSyncService : TimerBasedHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private const string LastSuccessfulSyncKey = "PosseSyncService.LastSuccessfulSync";

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
        var lastSuccessfulSync = await appConfiguration.TryGetValue(
            LastSuccessfulSyncKey,
            DateTimeOffset.MinValue
        );

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
            .ToDictionary(x => x.EnvPersonId, x => x);

        await SyncClients(personMapper, context, posseService, lastSuccessfulSync);
        await SyncAuthorizations(personMapper, context, posseService, lastSuccessfulSync);
        await appConfiguration.SetValue(LastSuccessfulSyncKey, DateTimeOffset.Now);

        Log.Information("Finished posse sync");
    }

    private static async Task SyncClients(
        Dictionary<string, PersonWithAuthorizations> personMapper,
        AppDbContext context,
        IPosseService posseService,
        DateTimeOffset lastSuccessfulSync
    )
    {
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
    }

    private static async Task SyncAuthorizations(
        Dictionary<string, PersonWithAuthorizations> personMapper,
        AppDbContext context,
        IPosseService posseService,
        DateTimeOffset lastSuccessfulSync
    )
    {
        var authorizations = await posseService.GetAuthorizations(
            lastSuccessfulSync,
            personMapper,
            context
        );
        context.Authorizations.AddRange(authorizations);
        await context.SaveChangesAsync();
    }
}
