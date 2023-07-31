using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Data.Extensions;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Shared.Services.Rules.Late;

internal class LateHuntReportRule : LateRule<HuntedActivity>
{
    public override IEnumerable<RuleType> ApplicableRuleTypes => new[] { RuleType.LateHuntReport };

    protected override async Task<DateTimeOffset?> GetDeadlineTimestamp(
        HuntedActivity activity,
        AppDbContext context
    )
    {
        var season = await HuntingSeason.GetSeason(activity, context);
        return activity switch
        {
            { Mortality.Species: Species.Moose }
            and { ActivityQueueItem.BagLimitEntry.IsThreshold: true }
                => activity.GetTimestampAfterKill(72),
            { Mortality.Species: Species.Moose }
            and (
                { ActivityQueueItem: null }
                or { ActivityQueueItem.BagLimitEntry.IsThreshold: false }
            )
                => activity.OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(),
            var _
                when activity.Mortality
                    is CaribouMortality
                        and {
                            LegalHerd: CaribouMortality.CaribouHerd.Fortymile
                                or CaribouMortality.CaribouHerd.Nelchina
                        }
                => activity.GetTimestampAfterKill(72),
            var _ when activity.Mortality is CaribouMortality and { LegalHerd: _ }
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
                    season.EndDate.Year,
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

    protected override Task<DateTimeOffset?> GetTimestampThatMustOccurBeforeTheDeadline(
        HuntedActivity _,
        Report report,
        AppDbContext __
    ) => Task.FromResult<DateTimeOffset?>(report.DateSubmitted);

    protected override Violation GenerateLateViolation(
        HuntedActivity activity,
        Report report,
        DateTimeOffset deadlineTimestamp
    )
    {
        return new Violation(
            activity,
            RuleType.LateHuntReport,
            SeverityType.Illegal,
            $"Report was submitted late for {activity.Mortality.Species.GetDisplayName().ToLower()}. Deadline was {deadlineTimestamp:yyyy-MM-dd}."
        );
    }

    protected override bool IsValidReportType(GeneralizedReportType type) =>
        type == GeneralizedReportType.Hunted;
}
