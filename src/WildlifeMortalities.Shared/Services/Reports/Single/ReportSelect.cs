using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Shared.Services.Reports.Single;

public static class ReportSelect
{
    public static IQueryable<Report> WithEntireGraph(this IQueryable<Report> reports)
    {
        return reports
            .Include(x => ((IndividualHuntedMortalityReport)x).HuntedActivity.Mortality)
            .Include(x => ((IndividualHuntedMortalityReport)x).HuntedActivity.GameManagementArea)
            .Include(x => ((SpecialGuidedHuntReport)x).Guide)
            .Include(x => ((SpecialGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Mortality)
            .Include(x => ((SpecialGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.GameManagementArea)
            .Include(x => ((OutfitterGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Mortality)
            .Include(x => ((OutfitterGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.GameManagementArea)
            .Include(x => ((OutfitterGuidedHuntReport)x).ChiefGuide)
            .Include(x => ((OutfitterGuidedHuntReport)x).AssistantGuides)
            .Include(x => ((OutfitterGuidedHuntReport)x).OutfitterArea)
            .Include(x => ((TrappedMortalitiesReport)x).RegisteredTrappingConcession)
            .Include(x => ((TrappedMortalitiesReport)x).TrappedActivities)
            .ThenInclude(x => x.Mortality);
    }

    public static async Task<ReportDetail?> GetDetails(
        this IQueryable<Report> report,
        int reportId,
        AppDbContext context
    )
    {
        var result = await report
            .WithEntireGraph()
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => x.Id == reportId);

        if (result == null)
        {
            return null;
        }

        var mortalities = result.GetMortalities();

        List<(int, BioSubmission)> bioSubmissions = new();
        foreach (var item in mortalities.OfType<IHasBioSubmission>())
        {
            BioSubmission? bioSubmission = item switch
            {
                AmericanBlackBearMortality
                    => await context.BioSubmissions
                        .OfType<AmericanBlackBearBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                CanadaLynxMortality
                    => await context.BioSubmissions
                        .OfType<CanadaLynxBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                CaribouMortality
                    => await context.BioSubmissions
                        .OfType<CaribouBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                ElkMortality
                    => await context.BioSubmissions
                        .OfType<ElkBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                GreyWolfMortality
                    => await context.BioSubmissions
                        .OfType<GreyWolfBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                GrizzlyBearMortality
                    => await context.BioSubmissions
                        .OfType<GrizzlyBearBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                MountainGoatMortality
                    => await context.BioSubmissions
                        .OfType<MountainGoatBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                MuleDeerMortality
                    => await context.BioSubmissions
                        .OfType<MuleDeerBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                ThinhornSheepMortality
                    => await context.BioSubmissions
                        .OfType<ThinhornSheepBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                WhiteTailedDeerMortality
                    => await context.BioSubmissions
                        .OfType<WhiteTailedDeerBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                WolverineMortality
                    => await context.BioSubmissions
                        .OfType<WolverineBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                WoodBisonMortality
                    => await context.BioSubmissions
                        .OfType<WoodBisonBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                _ => throw new InvalidOperationException()
            };

            if (bioSubmission != null)
            {
                if (bioSubmission is IHasHornMeasurementEntries submission)
                {
                    var firstAnnulusFound =
                        submission.HornMeasurementEntries.FirstOrDefault()?.Annulus ?? 1;

                    for (var annulus = 1; annulus < firstAnnulusFound; annulus++)
                    {
                        submission.HornMeasurementEntries.Insert(
                            annulus - 1,
                            new HornMeasurementEntry { IsBroomed = true, Annulus = annulus }
                        );
                    }

                    submission.HornMeasurementEntries = submission.HornMeasurementEntries.ToList();
                }

                bioSubmissions.Add((item.Id, bioSubmission));
            }
        }

        return new ReportDetail(result, bioSubmissions);
    }
}
