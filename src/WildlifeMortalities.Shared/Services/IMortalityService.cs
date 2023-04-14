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
    Task CreateReport(HumanWildlifeConflictMortalityReport report);
    Task CreateReport(IndividualHuntedMortalityReport report);
    Task CreateReport(OutfitterGuidedHuntReport report);
    Task CreateReport(SpecialGuidedHuntReport report);
    Task CreateDraftReport(string reportType, string reportContent, int personId);
    Task CreateBioSubmission(BioSubmission bioSubmission);
    Task UpdateBioSubmission(BioSubmission bioSubmission);
    Task<IEnumerable<GameManagementArea>> GetGameManagementAreas();
    Task<IEnumerable<OutfitterArea>> GetOutfitterAreas();
    Task<IEnumerable<RegisteredTrappingConcession>> GetRegisteredTrappingConcessions();
    Task CreateReport(TrappedMortalitiesReport report);
    Task UpdateDraftReport(string report, int reportId);
}
