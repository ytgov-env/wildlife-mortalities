using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Shared.Services;

public record ReportDetail(
    Report report,
    IEnumerable<(int mortalityId, BioSubmission submission)> bioSubmissions
);

public interface IMortalityService
{
    Task CreateReport(Report report);
    Task CreateDraftReport(string reportType, string reportContent, int personId);
    Task UpdateBioSubmissionAnalysis(BioSubmission bioSubmission);
    Task UpdateOrganicMaterialForBioSubmission(BioSubmission bioSubmission);
    Task<IEnumerable<GameManagementArea>> GetGameManagementAreas();
    Task<IEnumerable<OutfitterArea>> GetOutfitterAreas();
    Task<IEnumerable<RegisteredTrappingConcession>> GetRegisteredTrappingConcessions();
    Task UpdateDraftReport(string report, int reportId);
}
