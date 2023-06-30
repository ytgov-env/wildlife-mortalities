using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Extensions;

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
            .ThenInclude(x => x.Mortality)
            .Include(x => x.CreatedBy)
            .Include(x => x.LastModifiedBy)
            .AsSplitQuery();
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
            var bioSubmission = await context.BioSubmissions.GetBioSubmissionFromMortality(item);

            if (bioSubmission != null)
            {
                if (bioSubmission is IHasHornMeasurementEntries submission)
                {
                    submission.HornMeasurementEntries ??= new();

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
