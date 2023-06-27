using System.Diagnostics;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Rules.Late;

internal class LateHuntReportRule : Rule
{
    private static DateTimeOffset? GetLatestAcceptableReportTimestamp(HuntedActivity activity)
    {
        //if (activity?.ActivityQueueItem?.BagLimitEntry?.MaxValueForThreshold == null)
        //{
        //    throw new ArgumentException();
        //}
        return activity switch
        {
            { Mortality.Species: Species.Moose }
            and { IsThreshold: true }
                //{ Mortality.Species: Species.Moose }
                //and { ActivityQueueItem.BagLimitEntry.MaxValueForThreshold: 0 }
                => GetTimestampAfterKill(activity, 72),
            { Mortality.Species: Species.Moose }
            and { IsThreshold: false }
                => OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(
                    activity
                ),
            var _
                when activity.Mortality
                    is CaribouMortality
                        and {
                            Herd: CaribouMortality.CaribouHerd.Fortymile
                                or CaribouMortality.CaribouHerd.Nelchina
                        }
                => GetTimestampAfterKill(activity, 72),
            var _ when activity.Mortality is CaribouMortality and { Herd: _ }
                => OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(
                    activity
                ),
            { Mortality.Species: Species.WoodBison } => GetTimestampAfterKill(activity, 240),
            {
                Mortality.Species: Species.ThinhornSheep
                    or Species.MountainGoat
                    or Species.MuleDeer
                    or Species.GrizzlyBear
                    or Species.AmericanBlackBear
                    or Species.Coyote
                    or Species.Wolverine
            }
                => OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(
                    activity
                ),
            { Mortality.Species: Species.Elk } => GetTimestampAfterKill(activity, 72),
            //Todo: wolf
            //{ Mortality.Species: Species.GreyWolf } => new DateTimeOffset(activity.Mortality.DateOfDeath.Value.Year, ),
            _ => null,
        };
    }

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (report.GeneralizedReportType is not GeneralizedReportType.Hunted)
        {
            return RuleResult.NotApplicable;
        }

        var violations = new List<Violation>();
        var isUsed = false;
        foreach (
            var activity in report
                .GetActivities()
                .OfType<HuntedActivity>()
                .OrderBy(x => x.Mortality.DateOfDeath)
        )
        {
            var latestAcceptableReportTimestamp = GetLatestAcceptableReportTimestamp(activity);

            if (!latestAcceptableReportTimestamp.HasValue)
            {
                continue;
            }

            isUsed = true;
            if (report.DateSubmitted > latestAcceptableReportTimestamp.Value)
            {
                violations.Add(
                    new Violation(
                        activity,
                        Violation.RuleType.LateReport,
                        Violation.SeverityType.Illegal,
                        $"Report submitted after deadline for {activity.Mortality.Species.GetDisplayName()}. Deadline was {latestAcceptableReportTimestamp.Value:yyyy-MM-dd}."
                    )
                );
            }
        }

        return !isUsed
            ? RuleResult.NotApplicable
            : (violations.Any() ? RuleResult.IsIllegal(violations) : RuleResult.IsLegal);
    }

    private static DateTimeOffset GetTimestampAfterKill(HuntedActivity activity, int hours) =>
        activity.Mortality.DateOfDeath!.Value.AddDays(1).AddSeconds(-1).AddHours(hours);

    private static DateTimeOffset OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(
        HuntedActivity activity
    )
    {
        var dateOfDeath = activity.Mortality.DateOfDeath!.Value;
        var lastDayOfMonth = new DateTimeOffset(
            dateOfDeath.Year,
            dateOfDeath.Month,
            1,
            0,
            0,
            0,
            dateOfDeath.Offset
        )
            .AddMonths(1)
            .AddSeconds(-1);
        return lastDayOfMonth.AddDays(15);
    }
}
