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
    public static IQueryable<Report> WithActivities(this IQueryable<Report> reports)
    {
        return reports
            .Include(x => ((IndividualHuntedMortalityReport)x).Activity)
            .Include(x => ((SpecialGuidedHuntReport)x).HuntedActivities)
            .Include(x => ((OutfitterGuidedHuntReport)x).HuntedActivities)
            .Include(x => ((TrappedMortalitiesReport)x).TrappedActivities)
            .AsSingleQuery();
    }

    public static IQueryable<Report> WithEntireGraph(this IQueryable<Report> reports)
    {
        return reports
            .Include(x => ((IndividualHuntedMortalityReport)x).Activity.Mortality)
            .Include(x => ((IndividualHuntedMortalityReport)x).Activity.GameManagementArea)
            .Include(x => ((IndividualHuntedMortalityReport)x).Person)
            .Include(x => ((IndividualHuntedMortalityReport)x).Activity.Violations)
            .Include(x => ((IndividualHuntedMortalityReport)x).Activity.Authorizations)
            .Include(x => ((SpecialGuidedHuntReport)x).Client)
            .Include(x => ((SpecialGuidedHuntReport)x).Guide)
            .Include(x => ((SpecialGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Mortality)
            .Include(x => ((SpecialGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.GameManagementArea)
            .Include(x => ((SpecialGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Violations)
            .Include(x => ((SpecialGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Authorizations)
            .Include(x => ((OutfitterGuidedHuntReport)x).Client)
            .Include(x => ((OutfitterGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Mortality)
            .Include(x => ((OutfitterGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.GameManagementArea)
            .Include(x => ((OutfitterGuidedHuntReport)x).ChiefGuide)
            .Include(x => ((OutfitterGuidedHuntReport)x).AssistantGuides)
            .Include(x => ((OutfitterGuidedHuntReport)x).OutfitterArea)
            .Include(x => ((OutfitterGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Violations)
            .Include(x => ((OutfitterGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Authorizations)
            .Include(x => ((TrappedMortalitiesReport)x).Client)
            .Include(x => ((TrappedMortalitiesReport)x).RegisteredTrappingConcession)
            .Include(x => ((TrappedMortalitiesReport)x).TrappedActivities)
            .ThenInclude(x => x.Mortality)
            .Include(x => ((TrappedMortalitiesReport)x).TrappedActivities)
            .ThenInclude(x => x.Violations)
            .Include(x => ((TrappedMortalitiesReport)x).TrappedActivities)
            .ThenInclude(x => x.Authorizations)
            .Include(x => x.Season)
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
                if (bioSubmission is ThinhornSheepBioSubmission sheepSubmission)
                {
                    sheepSubmission.HornMeasurementEntries ??= new();

                    var firstAnnulusFound =
                        sheepSubmission.HornMeasurementEntries.FirstOrDefault()?.Annulus ?? 1;

                    for (var annulus = 1; annulus < firstAnnulusFound; annulus++)
                    {
                        sheepSubmission.HornMeasurementEntries.Insert(
                            annulus - 1,
                            new ThinhornSheepHornMeasurementEntry
                            {
                                IsBroomed = true,
                                Annulus = annulus
                            }
                        );
                    }

                    sheepSubmission.HornMeasurementEntries =
                        sheepSubmission.HornMeasurementEntries.ToList();
                }
                else if (bioSubmission is MountainGoatBioSubmission goatSubmission)
                {
                    goatSubmission.HornMeasurementEntries ??= new();

                    var firstAnnulusFound =
                        goatSubmission.HornMeasurementEntries.FirstOrDefault()?.Annulus ?? 1;

                    for (var annulus = 1; annulus < firstAnnulusFound; annulus++)
                    {
                        goatSubmission.HornMeasurementEntries.Insert(
                            annulus - 1,
                            new MountainGoatHornMeasurementEntry { Annulus = annulus }
                        );
                    }

                    goatSubmission.HornMeasurementEntries =
                        goatSubmission.HornMeasurementEntries.ToList();
                }

                bioSubmissions.Add((item.Id, bioSubmission));
            }
        }

        return new ReportDetail(result, bioSubmissions);
    }
}
