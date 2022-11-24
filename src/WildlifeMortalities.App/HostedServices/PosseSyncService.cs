using WildlifeMortalities.Data;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.HostedServices;

public class PosseSyncService : TimerBasedHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public PosseSyncService(IServiceProvider serviceProvider) : base(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(2))
    {
        _serviceProvider = serviceProvider;
    }

    protected override async void DoWork(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var posseClientService = scope.ServiceProvider.GetRequiredService<IPosseClientService>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Todo replace parameter with modifiedSinceDateTime retrieved from the database
        var apiResult = await posseClientService.RetrieveData(new DateTimeOffset());
    }
}
