using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Rules.Late;

public class LateBioSubmissionRule : LateRule<HarvestActivity>
{
    protected override async Task<DateTimeOffset?> GetDeadlineTimestamp(
        HarvestActivity activity,
        AppDbContext context
    )
    {
        var season = await HuntingSeason.GetSeason(activity, context);

        return activity switch
        {
            var _
                when activity.Mortality
                    is CaribouMortality
                        and {
                            Herd: CaribouMortality.CaribouHerd.Fortymile
                                or CaribouMortality.CaribouHerd.Nelchina
                        }
                => activity.GetTimestampAfterKill(72),
            { Mortality.Species: Species.Elk } => activity.GetTimestampAfterKill(72),
            HuntedActivity huntedActivity
            and {
                Mortality.Species: Species.WoodBison
                    or Species.ThinhornSheep
                    or Species.MountainGoat
                    or Species.MuleDeer
                    or Species.GrizzlyBear
                    or Species.AmericanBlackBear
                    or Species.Wolverine
            }
                => huntedActivity.OccuredMoreThanFifteenDaysAfterTheEndOfTheMonthInWhichTheAnimalWasKilled(),
            TrappedActivity trappedActivity
            and { Mortality.Species: Species.CanadaLynx or Species.Wolverine }
                => trappedActivity.OccuredMoreThanFifteenDaysAfterTheEndOfTheTrappingSeasonForSpecies(),
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
            _ => null
        };
    }

    protected override async Task<DateTimeOffset?> GetTimestampThatMustOccurBeforeTheDeadline(
        HarvestActivity activity,
        Report _,
        AppDbContext context
    )
    {
        var bioSubmission = await context.BioSubmissions.FirstAsync(x => x.Id == activity.Id);
        return bioSubmission?.DateSubmitted;
    }

    protected override Violation GenerateLateViolation(
        HarvestActivity activity,
        Report _,
        DateTimeOffset deadlineTimestamp
    )
    {
        return new(
            activity,
            Violation.RuleType.LateBioSubmission,
            Violation.SeverityType.Illegal,
            $"Biological submission was submitted late for {activity.Mortality.Species.GetDisplayName().ToLower()}. Deadline was {deadlineTimestamp:yyyy-MM-dd}."
        );
    }

    protected override bool IsValidReportType(GeneralizedReportType type) =>
        type is GeneralizedReportType.Hunted or GeneralizedReportType.Trapped;
}
