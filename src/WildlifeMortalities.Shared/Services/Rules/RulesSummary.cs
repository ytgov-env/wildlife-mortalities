using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Shared.Services.Rules;

public class RulesSummary
{
    public Report Report { get; set; } = null!;
    public List<Violation> Violations { get; set; } = null!;
    public List<Authorization> Authorizations { get; set; } = null!;

    public static async Task Generate(Report report, Report? existingReport, AppDbContext context)
    {
        if (existingReport != null)
        {
            var existingViolation = existingReport
                .GetActivities()
                .SelectMany(x => x.Violations)
                .ToArray();
            context.RemoveRange(existingViolation);
            await ResetRules(existingReport, context);
        }

        var rulesSummary = new RulesSummary
        {
            Report = report,
            Violations = new List<Violation>(),
            Authorizations = new List<Authorization>()
        };

        foreach (var item in RulesEngine.Rules)
        {
            var result = await item.Process(report, context);
            rulesSummary.Violations.AddRange(result.Violations);
            rulesSummary.Authorizations.AddRange(result.Authorizations);
        }

        // Ensure uniqueness of activities when adding or updating
        var modifiedActivities = context.ChangeTracker
            .Entries<Activity>()
            .ToDictionary(x => x.Entity.Id, x => x.Entity);
        foreach (var item in rulesSummary.Violations)
        {
            if (modifiedActivities.TryGetValue(item.Activity.Id, out var activity))
            {
                item.Activity = activity;
            }
        }

        context.AddRange(rulesSummary.Violations);
    }

    private static async Task ResetRules(Report report, AppDbContext context)
    {
        foreach (var item in RulesEngine.Rules)
        {
            await item.Reset(report, context);
        }
    }
}
