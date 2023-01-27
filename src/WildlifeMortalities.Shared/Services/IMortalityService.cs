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
    Task CreateReport(HumanWildlifeConflictMortalityReport report);
    Task CreateReport(IndividualHuntedMortalityReport report);
    Task CreateReport(OutfitterGuidedHuntReport report);
    Task CreateReport(SpecialGuidedHuntReport report);
    Task<IEnumerable<Report>> GetAllReports(int start = 0, int length = 10);

    Task<IEnumerable<Report>> GetReportsByEnvClientId(
        string envClientId,
        int start = 0,
        int length = 10
    );

    Task<int> CountAllReports();
    Task<int> CountReportsByEnvClientId(string envClientId);
    Task<ReportDetail?> GetReport(int id);
    Task CreateBioSubmission(BioSubmission bioSubmission);
    Task UpdateBioSubmission(BioSubmission bioSubmission);
}
