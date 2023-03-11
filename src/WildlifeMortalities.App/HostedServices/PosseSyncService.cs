using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.HostedServices;

public class PosseSyncService : TimerBasedHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public PosseSyncService(IServiceProvider serviceProvider)
        : base(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(2)) =>
        _serviceProvider = serviceProvider;

    protected override async void DoWork(object? state)
    {
        Log.Information("Starting posse sync");
        using var scope = _serviceProvider.CreateScope();
        var posseClientService = scope.ServiceProvider.GetRequiredService<IPosseService>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var clientMapper = context.People
            .OfType<Client>()
            .Include(x => x.DraftReports)
            .Include(x => x.SpecialGuideLicencesAsClient)
            .Include(x => x.IndividualHuntedMortalityReports)
            .Include(x => x.OutfitterGuidedHuntReportsAsChiefGuide)
            .Include(x => x.OutfitterGuidedHuntReportsAsAssistantGuide)
            .Include(x => x.OutfitterGuidedHuntReportsAsClient)
            .Include(x => x.SpecialGuidedHuntReportsAsGuide)
            .Include(x => x.SpecialGuidedHuntReportsAsClient)
            .Include(x => x.TrappedMortalitiesReports)
            .Include(x => x.Authorizations)
            .ToDictionary(x => x.EnvClientId, x => x);

        var recentlyModifiedClients = await posseClientService.RetrieveClientData(
            new DateTimeOffset(new DateTime(2023, 02, 15), new TimeSpan(-7, 0, 0))
        );

        foreach (
            var (client, previousEnvClientIds) in recentlyModifiedClients.OrderBy(
                x => x.Item1.LastModifiedDateTime
            )
        )
        {
            // Determine which are existing clients, and update them
            if (clientMapper.TryGetValue(client.EnvClientId, out var clientInDatabase))
            {
                clientInDatabase.Update(client);
                await context.SaveChangesAsync();
            }
            // Or, add a new client
            else
            {
                context.Add(client);
                await context.SaveChangesAsync();
                clientMapper.Add(client.EnvClientId, client);
                clientInDatabase = client;
            }

            foreach (var envClientId in previousEnvClientIds)
            {
                // Determine if client was merged in POSSE, and merge them
                if (clientMapper.TryGetValue(envClientId, out var clientToBeMerged))
                {
                    clientInDatabase.Merge(clientToBeMerged);
                    context.People.Remove(clientToBeMerged);
                    clientMapper.Remove(clientToBeMerged.EnvClientId);
                    await context.SaveChangesAsync();
                }
            }
        }

        var authorizations = await posseClientService.RetrieveAuthorizationData(
            new DateTimeOffset(new DateTime(2023, 02, 15), new TimeSpan(-7, 0, 0))
        );

        foreach (var (auth, envClientId) in authorizations)
        {
            auth.Client = clientMapper[envClientId];
            context.Authorizations.Add(auth);
        }

        //await context.SaveChangesAsync();
        Log.Information("Finished posse sync");
    }
}
