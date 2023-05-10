using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Shared.Services;

public record ReportDetail(
    Report Report,
    IEnumerable<(int mortalityId, BioSubmission submission)> BioSubmissions
);

public interface IMortalityService
{
    Task CreateReport(Report report, int userId);
    Task UpdateReport(Report report, int userId);
    Task CreateDraftReport(string reportType, string reportContent, int personId);
    Task UpdateDraftReport(string report, int reportId);

    Task UpdateBioSubmissionAnalysis(BioSubmission bioSubmission);
    Task UpdateOrganicMaterialForBioSubmission(BioSubmission bioSubmission);
    Task<IEnumerable<GameManagementArea>> GetGameManagementAreas();
    Task<IEnumerable<OutfitterArea>> GetOutfitterAreas();
    Task<IEnumerable<RegisteredTrappingConcession>> GetRegisteredTrappingConcessions();
}
