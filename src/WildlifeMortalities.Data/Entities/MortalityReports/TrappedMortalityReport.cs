using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.MortalityReports;

public class TrappedMortalityReport : MortalityReport
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public int TrappingConcessionAreaId { get; set; }
    public TrappingConcessionArea TrappingConcessionArea { get; set; } = null!;
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset DateCompleted { get; set; }
    public int? AuthorizationId { get; set; }
    public Authorization? Authorization { get; set; }
    public TrappedMortalityReportStatus Status { get; set; }
    public string Comment { get; set; } = string.Empty;
    public List<Violation> Violations { get; set; } = null!;
}

public enum TrappedMortalityReportStatus
{
}
