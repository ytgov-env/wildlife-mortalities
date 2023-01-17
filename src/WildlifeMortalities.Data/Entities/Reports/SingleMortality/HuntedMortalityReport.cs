using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class HuntedMortalityReport : MortalityReport, IHasClientReporter
{
    public int GameManagementAreaId { get; set; }
    public GameManagementArea GameManagementArea { get; set; } = null!;
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset DateCompleted { get; set; }
    public string Landmark { get; set; } = string.Empty;
    public int? AuthorizationId { get; set; }
    public Authorization? Authorization { get; set; }

    public OutfitterGuidedHuntReport? OutfitterGuidedHuntReport { get; set; }

    public SpecialGuidedHuntReport? SpecialGuidedHuntReport { get; set; }

    // Todo add status resolution logic
    public HuntedMortalityReportStatus Status { get; set; } = HuntedMortalityReportStatus.Complete;
    public string Comment { get; set; } = string.Empty;
    public List<Violation> Violations { get; set; } = null!;
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}

public class HuntedMortalityReportConfig : IEntityTypeConfiguration<HuntedMortalityReport>
{
    public void Configure(EntityTypeBuilder<HuntedMortalityReport> builder) =>
        builder.ToTable("Reports");
}

public enum HuntedMortalityReportStatus
{
    Complete = 10,
    WaitingOnClient = 20
}
