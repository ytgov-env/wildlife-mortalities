using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.MortalityReports;

public class HuntedMortalityReport : MortalityReport
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public int GameManagementAreaId { get; set; }
    public GameManagementArea GameManagementArea { get; set; } = null!;
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset DateCompleted { get; set; }
    public string Landmark { get; set; } = string.Empty;
    public int? AuthorizationId { get; set; }
    public Authorization? Authorization { get; set; }
    public HuntedMortalityReportStatus Status { get; set; }
    public string Comment { get; set; } = string.Empty;
    public List<Violation> Violations { get; set; } = null!;
}

public class HuntedMortalityReportConfig : IEntityTypeConfiguration<HuntedMortalityReport>
{
    public void Configure(EntityTypeBuilder<HuntedMortalityReport> builder)
    {
    }
}

public enum HuntedMortalityReportStatus
{
    Complete = 10,
    CompleteWithViolations = 20,
    WaitingOnClient = 30
}
