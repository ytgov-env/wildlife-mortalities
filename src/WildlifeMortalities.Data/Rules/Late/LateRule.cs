using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities;

namespace WildlifeMortalities.Data.Rules.Late;

public abstract class LateRule<TActivity> : Rule
    where TActivity : HarvestActivity
{
    protected abstract Task<DateTimeOffset?> GetDeadlineTimestamp(
        TActivity activity,
        AppDbContext context
    );
    protected abstract bool IsValidReportType(GeneralizedReportType type);
    protected abstract Task<DateTimeOffset?> GetTimestampThatMustOccurBeforeTheDeadline(
        TActivity activity,
        Report report,
        AppDbContext context
    );
    protected abstract Violation GenerateLateViolation(
        TActivity activity,
        Report report,
        DateTimeOffset deadlineTimestamp
    );

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (!IsValidReportType(report.GeneralizedReportType))
        {
            return RuleResult.NotApplicable;
        }

        var violations = new List<Violation>();
        var isApplicable = false;
        foreach (
            var activity in report
                .GetActivities()
                .OfType<TActivity>()
                .OrderBy(x => x.Mortality.DateOfDeath)
        )
        {
            var deadlineTimestamp = await GetDeadlineTimestamp(activity, context);

            if (!deadlineTimestamp.HasValue)
            {
                continue;
            }

            var timestampThatMustOccurBeforeTheDeadline =
                await GetTimestampThatMustOccurBeforeTheDeadline(activity, report, context);
            if (!timestampThatMustOccurBeforeTheDeadline.HasValue)
            {
                continue;
            }
            isApplicable = true;
            if (timestampThatMustOccurBeforeTheDeadline.Value > deadlineTimestamp.Value)
            {
                violations.Add(GenerateLateViolation(activity, report, deadlineTimestamp.Value));
            }
        }

        return !isApplicable
            ? RuleResult.NotApplicable
            : (violations.Any() ? RuleResult.IsIllegal(violations) : RuleResult.IsLegal);
    }
}
