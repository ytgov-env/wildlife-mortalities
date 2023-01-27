using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class HuntedActivity : Activity
{
    public int GameManagementAreaId { get; set; }
    public GameManagementArea GameManagementArea { get; set; } = null!;
    public string Landmark { get; set; } = string.Empty;

    public int? IndividualHuntedMortalityReportId { get; set; }
    public IndividualHuntedMortalityReport? IndividualHuntedMortalityReport { get; set; }
    public int? OutfitterGuidedHuntReportId { get; set; }
    public OutfitterGuidedHuntReport? OutfitterGuidedHuntReport { get; set; }
    public int? SpecialGuidedHuntReportId { get; set; }
    public SpecialGuidedHuntReport? SpecialGuidedHuntReport { get; set; }
}

public class HuntedActivityConfig : IEntityTypeConfiguration<HuntedActivity>
{
    public void Configure(EntityTypeBuilder<HuntedActivity> builder) => builder.ToTable("Activities");
}

public enum HuntedMortalityReportStatus
{
    Complete = 10,
    WaitingOnClient = 20
}
