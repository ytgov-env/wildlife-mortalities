using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Rules;

public abstract class Rule
{
    public abstract Task<RuleResult> Process(Report report, AppDbContext context);
}
