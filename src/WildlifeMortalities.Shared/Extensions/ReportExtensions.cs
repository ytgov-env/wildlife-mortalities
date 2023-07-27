using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Shared.Extensions;

internal static class ReportExtensions
{
    public static async Task<ReportDetail> GetDetails(this Report report, AppDbContext context)
    {
        var mortalities = report.GetMortalities();

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

        return new ReportDetail(report, bioSubmissions);
    }
}
