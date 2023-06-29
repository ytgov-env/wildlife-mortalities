using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Rules.Late;

internal class LateHuntReportRule : LateRule<HuntedActivity>
{
    protected override DateTimeOffset? GetDeadlineTimestamp(HuntedActivity activity)
    {
        if (activity?.ActivityQueueItem?.BagLimitEntry == null)
        {
            throw new ArgumentException(
                $"{nameof(BagLimitEntry)} must not be null. Probably a navigation property is not included.",
                nameof(activity)
            );
        }
        return activity switch
        {
            { Mortality.Species: Species.Moose }
            and { ActivityQueueItem.BagLimitEntry.IsThreshold: true }
                => activity.GetTimestampAfterKill(72),
            { Mortality.Species: Species.Moose }
            and { ActivityQueueItem.BagLimitEntry.IsThreshold: false }
                => activity.OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(),
            var _
                when activity.Mortality
                    is CaribouMortality
                        and {
                            Herd: CaribouMortality.CaribouHerd.Fortymile
                                or CaribouMortality.CaribouHerd.Nelchina
                        }
                => activity.GetTimestampAfterKill(72),
            var _ when activity.Mortality is CaribouMortality and { Herd: _ }
                => activity.OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(),
            { Mortality.Species: Species.WoodBison } => activity.GetTimestampAfterKill(240),
            {
                Mortality.Species: Species.ThinhornSheep
                    or Species.MountainGoat
                    or Species.MuleDeer
                    or Species.GrizzlyBear
                    or Species.AmericanBlackBear
                    or Species.Coyote
                    or Species.Wolverine
            }
                => activity.OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(),
            { Mortality.Species: Species.Elk } => activity.GetTimestampAfterKill(72),
            { Mortality.Species: Species.GreyWolf }
                => new DateTimeOffset(
                    activity.ActivityQueueItem.BagLimitEntry.GetSeason().EndDate.Year,
                    4,
                    15,
                    23,
                    59,
                    59,
                    TimeSpan.FromHours(-7)
                ),
            _ => null,
        };
    }

    protected override Task<DateTimeOffset> GetTimestampThatMustOccurBeforeTheDeadline(
        HuntedActivity _,
        Report report,
        AppDbContext __
    ) => Task.FromResult(report.DateSubmitted);

    protected override Violation GenerateViolation(
        HuntedActivity activity,
        Report report,
        DateTimeOffset deadlineTimestamp
    )
    {
        return new Violation(
            activity,
            Violation.RuleType.LateReport,
            Violation.SeverityType.Illegal,
            $"Report submitted after deadline for {activity.Mortality.Species.GetDisplayName().ToLower()}. Deadline was {deadlineTimestamp:yyyy-MM-dd}."
        );
    }

    protected override bool IsValidReportType(GeneralizedReportType type) =>
        type == GeneralizedReportType.Hunted;
}
