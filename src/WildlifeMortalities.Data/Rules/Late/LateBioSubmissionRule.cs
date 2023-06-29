using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Rules.Late;

public class LateBioSubmissionRule : LateRule<HarvestActivity>
{
    protected override DateTimeOffset? GetDeadlineTimestamp(HarvestActivity activity)
    {
        if (activity?.ActivityQueueItem?.BagLimitEntry == null)
        {
            throw new ArgumentException(
                $"{nameof(BagLimitEntry)} must not be null. A navigation property was not included.",
                nameof(activity)
            );
        }
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
            and { Mortality.Species: Species.CanadaLynx or Species.Wolverine or Species.GreyWolf }
                => trappedActivity.OccuredMoreThanFifteenDaysAfterTheEndOfTheTrappingSeasonForSpecies(),
            //Todo: wolf hunting
            _ => null
        };
    }

    protected override async Task<DateTimeOffset> GetTimestampThatMustOccurBeforeTheDeadline(
        HarvestActivity activity,
        Report _,
        AppDbContext context
    )
    {
        var bioSubmission = await context.BioSubmissions.FirstAsync(x => x.Id == activity.Id);
        return bioSubmission?.DateSubmitted ?? DateTimeOffset.Now;
    }

    protected override Violation GenerateViolation(
        HarvestActivity activity,
        Report _,
        DateTimeOffset latestAcceptableTimestamp
    ) =>
        new(
            activity,
            Violation.RuleType.LateBioSubmission,
            Violation.SeverityType.Illegal,
            $"Biological submission was not submitted for {activity.Mortality.Species.GetDisplayName().ToLower()}. Deadline was {latestAcceptableTimestamp:yyyy-MM-dd}."
        );

    protected override bool IsValidReportType(GeneralizedReportType type) =>
        type is GeneralizedReportType.Hunted or GeneralizedReportType.Trapped;
}
