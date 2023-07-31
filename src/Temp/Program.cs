using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EnvWildlifeMortalities;Integrated Security=True;"
    )
    .EnableSensitiveDataLogging()
    .Options;

using var context = new AppDbContext(options);

var reports = await JsonSerializer.DeserializeAsync<List<IndividualHuntedMortalityReport>>(
    File.OpenRead(@"C:\Users\jhodgins\OneDrive - Government of Yukon\Documents\reports.json")
);

foreach (var item in reports)
{
    item.SeasonId = item.Season.Id;
    item.Season = null!;

    item.Id = 0;
    foreach (var activity in item.GetActivities())
    {
        activity.Id = 0;
        activity.Mortality.Id = 0;

        if (activity.Mortality is AmericanBlackBearMortality blackBearMortality)
        {
            blackBearMortality.BioSubmission.Id = 0;
            if (
                blackBearMortality.BioSubmission.HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis()
            )
            {
                blackBearMortality.BioSubmission.RequiredOrganicMaterialsStatus =
                    BioSubmissionRequiredOrganicMaterialsStatus.Submitted;

                blackBearMortality.BioSubmission.AnalysisStatus =
                    BioSubmissionAnalysisStatus.NotStarted;
            }
            else
            {
                blackBearMortality.BioSubmission.AnalysisStatus =
                    BioSubmissionAnalysisStatus.NotStarted;
                blackBearMortality.BioSubmission.RequiredOrganicMaterialsStatus =
                    BioSubmissionRequiredOrganicMaterialsStatus.NotStarted;
            }
        }
        else if (activity.Mortality is GrizzlyBearMortality grizzlyBearMortality)
        {
            grizzlyBearMortality.BioSubmission.Id = 0;
            if (
                grizzlyBearMortality.BioSubmission.HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis()
            )
            {
                grizzlyBearMortality.BioSubmission.RequiredOrganicMaterialsStatus =
                    BioSubmissionRequiredOrganicMaterialsStatus.Submitted;
                grizzlyBearMortality.BioSubmission.AnalysisStatus =
                    BioSubmissionAnalysisStatus.NotStarted;
            }
            else if (
                grizzlyBearMortality.BioSubmission.IsSkullProvided == true
                || grizzlyBearMortality.BioSubmission.IsEvidenceOfSexAttached == true
            )
            {
                grizzlyBearMortality.BioSubmission.AnalysisStatus =
                    BioSubmissionAnalysisStatus.NotStarted;
                grizzlyBearMortality.BioSubmission.RequiredOrganicMaterialsStatus =
                    BioSubmissionRequiredOrganicMaterialsStatus.PartiallySubmitted;
            }
            else
            {
                grizzlyBearMortality.BioSubmission.AnalysisStatus =
                    BioSubmissionAnalysisStatus.NotStarted;
                grizzlyBearMortality.BioSubmission.RequiredOrganicMaterialsStatus =
                    BioSubmissionRequiredOrganicMaterialsStatus.NotStarted;
            }
        }
    }
}

context.AddRange(reports);

await context.SaveChangesAsync();
