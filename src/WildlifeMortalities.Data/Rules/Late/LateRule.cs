using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Extensions;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Data.Rules.Late;

public abstract class LateRule<TActivity> : Rule
    where TActivity : HarvestActivity
{
    protected abstract DateTimeOffset? GetDeadlineTimestamp(TActivity activity);
    protected abstract bool IsValidReportType(GeneralizedReportType type);
    protected abstract Task<DateTimeOffset?> GetTimestampThatMustOccurBeforeTheDeadline(
        TActivity activity,
        Report report,
        AppDbContext context
    );
    protected abstract Violation GenerateViolation(
        TActivity activity,
        Report report,
        DateTimeOffset latestAcceptableTimestamp
    );

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (!IsValidReportType(report.GeneralizedReportType))
        {
            return RuleResult.NotApplicable;
        }

        var violations = new List<Violation>();
        var isUsed = false;
        foreach (
            var activity in report
                .GetActivities()
                .OfType<TActivity>()
                .OrderBy(x => x.Mortality.DateOfDeath)
        )
        {
            var deadlineTimestamp = GetDeadlineTimestamp(activity);

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
            isUsed = true;
            if (timestampThatMustOccurBeforeTheDeadline.Value > deadlineTimestamp.Value)
            {
                violations.Add(GenerateViolation(activity, report, deadlineTimestamp.Value));
            }
        }

        return !isUsed
            ? RuleResult.NotApplicable
            : (violations.Any() ? RuleResult.IsIllegal(violations) : RuleResult.IsLegal);
    }
}
