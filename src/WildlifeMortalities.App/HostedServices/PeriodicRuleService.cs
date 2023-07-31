using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services.Reports.Single;
using WildlifeMortalities.Shared.Services.Rules;

namespace WildlifeMortalities.App.HostedServices;

public class PeriodicRuleService : TimerBasedHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public PeriodicRuleService(IServiceProvider serviceProvider)
        : base(new TimeOnly(3, 0, 0), TimeSpan.FromHours(24))
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task DoWork(object? state)
    {
        Log.Information("Starting periodic rule service");
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var threshold = DateTimeOffset.Now.AddDays(-400);

        var reports = context.Reports
            .Where(
                x =>
                    x is IndividualHuntedMortalityReport
                    || x is SpecialGuidedHuntReport
                    || x is OutfitterGuidedHuntReport
                    || x is TrappedMortalitiesReport
            )
            .Where(x => x.DateSubmitted > threshold)
            .WithEntireGraph();

        foreach (var report in reports)
        {
            await RulesSummary.ResetRules(report, RulesEngine.PeriodicRules, context);
            await RulesSummary.Generate(report, RulesEngine.PeriodicRules, context);
        }

        await context.SaveChangesAsync();

        Log.Information("Finished periodic rule service");
    }
}
