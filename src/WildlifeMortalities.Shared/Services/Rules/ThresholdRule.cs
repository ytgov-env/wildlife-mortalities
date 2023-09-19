using System.Linq;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Extensions;
using WildlifeMortalities.Data.Migrations;

namespace WildlifeMortalities.Shared.Services.Rules;

internal class ThresholdRule : Rule
{
    public override IEnumerable<Violation.RuleType> ApplicableRuleTypes =>
        new[] { Violation.RuleType.ThresholdExceeded };

    private static async Task<List<BagLimitEntry>> ResetAndPrepare(
        Report report,
        AppDbContext context
    )
    {
        var entries = new List<BagLimitEntry>();
        foreach (
            var activity in report
                .GetActivities()
                .OfType<HarvestActivity>()
                .OrderBy(x => x.Mortality.DateOfDeath)
        )
        {
            var bagLimitEntry = (
                await context.BagLimitEntries
                    .Include(x => x.MaxValuePerPersonSharedWith)
                    .Include(x => x.ActivityQueue)
                    .ThenInclude(x => x.Activity)
                    .ThenInclude(x => x.Mortality)
                    .Include(x => x.ActivityQueue)
                    .ThenInclude(x => x.Activity)
                    .ThenInclude(x => x.Violations)
                    .Include(x => ((HuntingBagLimitEntry)x).Areas)
                    .Include(x => ((HuntingBagLimitEntry)x).Season)
                    .Include(x => ((TrappingBagLimitEntry)x).Concessions)
                    .Include(x => ((TrappingBagLimitEntry)x).Season)
                    .Where(x => x.Species == activity.Mortality.Species)
                    .AsSplitQuery()
                    .ToArrayAsync()
            ).SingleOrDefault(x => x.Matches(activity, report));

            if (bagLimitEntry == null || !bagLimitEntry.IsThreshold)
            {
                continue;
            }

            foreach (var item in bagLimitEntry.ActivityQueue)
            {
                if (item.Activity.Violations == null)
                {
                    continue;
                }

                item.Activity.Violations.RemoveAll(
                    x => x.Rule == Violation.RuleType.ThresholdExceeded
                );
            }

            entries.Add(bagLimitEntry);
        }
        return entries;
    }

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (
            report.GeneralizedReportType
            is not GeneralizedReportType.Hunted
                and not GeneralizedReportType.Trapped
        )
        {
            return RuleResult.NotApplicable;
        }

        var bagLimitEntries = await ResetAndPrepare(report, context);
        var violations = new List<Violation>();

        foreach (var bagLimitEntry in bagLimitEntries)
        {
            if (bagLimitEntry.ActivityQueue.Count < bagLimitEntry.MaxValueForThreshold)
            {
                continue;
            }

            var dayThresholdWasMet = DateOnly.FromDateTime(
                bagLimitEntry.ActivityQueue
                    .First(x => x.Position == bagLimitEntry.MaxValueForThreshold!.Value)
                    .Activity.Mortality.DateOfDeath!.Value.Date
            );

            var illegalThresholdTimestamp = dayThresholdWasMet.AddDays(3);

            violations.AddRange(
                bagLimitEntry.ActivityQueue
                    .GroupBy(
                        x =>
                            DateOnly.FromDateTime(
                                x.Activity.Mortality.DateOfDeath!.Value.Date
                            ) switch
                            {
                                var input
                                    when (
                                        input >= dayThresholdWasMet
                                        && input < illegalThresholdTimestamp
                                    )
                                    => Violation.SeverityType.PotentiallyIllegal,
                                var input when input >= illegalThresholdTimestamp
                                    => Violation.SeverityType.Illegal,
                                _ => null as Violation.SeverityType?
                            }
                    )
                    .Where(x => x.Key is not null)
                    .SelectMany(
                        x =>
                            x.Select(
                                y =>
                                    new Violation(
                                        y.Activity,
                                        Violation.RuleType.ThresholdExceeded,
                                        x.Key!.Value,
                                        bagLimitEntry
                                            .GetType()
                                            .IsAssignableTo(typeof(HuntingBagLimitEntry))
                                            ? $"Threshold exceeded for {y.Activity.Mortality.Species.GetDisplayName().ToLower()} in {((HuntingBagLimitEntry)bagLimitEntry).Areas.AreasToString()}. Threshold of {bagLimitEntry.MaxValueForThreshold} was reached on {dayThresholdWasMet.ToString(Constants.FormatStrings.StandardDateFormat)}."
                                            : $"Threshold exceeded for {y.Activity.Mortality.Species.GetDisplayName().ToLower()} in {((TrappingBagLimitEntry)bagLimitEntry).Concessions.ConcessionsToString()}. Threshold of {bagLimitEntry.MaxValueForThreshold} was reached on {dayThresholdWasMet.ToString(Constants.FormatStrings.StandardDateFormat)}."
                                    )
                            )
                    )
            );
        }

        return violations.Count == 0 ? RuleResult.IsLegal : RuleResult.IsIllegal(violations);
    }

    public override async Task Reset(Report report, AppDbContext context)
    {
        if (
            report.GeneralizedReportType
            is not GeneralizedReportType.Hunted
                and not GeneralizedReportType.Trapped
        )
        {
            return;
        }

        await ResetAndPrepare(report, context);
    }
}
