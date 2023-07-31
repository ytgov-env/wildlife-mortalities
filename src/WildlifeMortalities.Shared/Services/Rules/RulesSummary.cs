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

    public static async Task Generate(Report report, IEnumerable<Rule> rules, AppDbContext context)
    {
        var rulesSummary = new RulesSummary
        {
            Report = report,
            Violations = new List<Violation>(),
            Authorizations = new List<Authorization>()
        };

        foreach (var item in rules)
        {
            var result = await item.Process(report, context);
            rulesSummary.Violations.AddRange(result.Violations);
            rulesSummary.Authorizations.AddRange(result.Authorizations);
        }

        context.AddRange(rulesSummary.Violations);
    }

    public static async Task GenerateAll(Report report, AppDbContext context) =>
        await Generate(report, RulesEngine.Rules, context);

    public static async Task ResetRules(
        Report report,
        IEnumerable<Rule> rules,
        AppDbContext context
    )
    {
        var existingViolation = report.GetActivities().SelectMany(x => x.Violations).ToArray();
        var applicableTypes = rules.SelectMany(x => x.ApplicableRuleTypes);

        context.RemoveRange(existingViolation.Where(x => applicableTypes.Contains(x.Rule)));

        foreach (var rule in rules)
        {
            await rule.Reset(report, context);
        }
    }

    public static async Task ResetAllRules(Report report, AppDbContext context) =>
        await ResetRules(report, RulesEngine.Rules, context);
}
