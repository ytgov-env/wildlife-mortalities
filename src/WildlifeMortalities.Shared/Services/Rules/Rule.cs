using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Shared.Services.Rules;

public abstract class Rule
{
    public abstract Task<RuleResult> Process(Report report, AppDbContext context);

    public virtual Task Reset(Report report, AppDbContext context) => Task.CompletedTask;
}
