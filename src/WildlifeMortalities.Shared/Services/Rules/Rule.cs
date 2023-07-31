using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Reports;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Shared.Services.Rules;

public abstract class Rule
{
    public abstract Task<RuleResult> Process(Report report, AppDbContext context);

    public virtual Task Reset(Report report, AppDbContext context) => Task.CompletedTask;

    public abstract IEnumerable<RuleType> ApplicableRuleTypes { get; }
}
