using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Shared.Services.Rules.Late;

public class LateBioSubmissionRule : LateRule<HarvestActivity>
{
    protected override async Task<DateTimeOffset?> GetDeadlineTimestamp(
        HarvestActivity activity,
        AppDbContext context
    )
    {
        Season season = activity switch
        {
            HuntedActivity hunted => await HuntingSeason.GetSeason(hunted, context),
            TrappedActivity trapped => await TrappingSeason.GetSeason(trapped, context),
            _ => throw new UnreachableException()
        };

        return activity switch
        {
            var _
                when activity.Mortality
                    is CaribouMortality
                        and {
                            LegalHerd: CaribouMortality.CaribouHerd.Fortymile
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
        if (activity.Mortality is IHasBioSubmission mortality)
        {
            var bioSubmission =
                await context.BioSubmissions.GetBioSubmissionFromMortality(mortality)
                ?? context.ChangeTracker
                    .Entries<BioSubmission>()
                    .Select(x => x.Entity)
                    .GetBioSubmissionFromMortality(mortality)
                ?? throw new Exception(
                    "Expected mortality to have a bio submission, but no bio submission found."
                );
            return bioSubmission.DateSubmitted;
        }

        return null;
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
