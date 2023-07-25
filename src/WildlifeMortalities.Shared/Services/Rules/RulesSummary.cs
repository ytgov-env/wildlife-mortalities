using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Shared.Services.Rules;

public class RulesSummary
{
    public Report Report { get; set; } = null!;
    public List<Violation> Violations { get; set; } = null!;
    public List<Authorization> Authorizations { get; set; } = null!;

    public static async Task Generate(Report report, AppDbContext context)
    {
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

        context.AddRange(rulesSummary.Violations);
    }

    public static async Task ResetRules(Report report, AppDbContext context)
    {
        var existingViolation = report.GetActivities().SelectMany(x => x.Violations).ToArray();

        context.RemoveRange(existingViolation);

        foreach (var rule in RulesEngine.Rules)
        {
            await rule.Reset(report, context);
        }
    }
}
