using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class HuntedActivity : HarvestActivity
{
    [Column($"{nameof(HuntedActivity)}_{nameof(GameManagementAreaId)}")]
    public int GameManagementAreaId { get; set; }
    public GameManagementArea GameManagementArea { get; set; } = null!;

    [Column($"{nameof(HuntedActivity)}_{nameof(Landmark)}")]
    public string Landmark { get; set; } = string.Empty;

    [Column($"{nameof(HuntedActivity)}_{nameof(IndividualHuntedMortalityReportId)}")]
    public int? IndividualHuntedMortalityReportId { get; set; }
    public IndividualHuntedMortalityReport? IndividualHuntedMortalityReport { get; set; }

    [Column($"{nameof(HuntedActivity)}_{nameof(OutfitterGuidedHuntReportId)}")]
    public int? OutfitterGuidedHuntReportId { get; set; }
    public OutfitterGuidedHuntReport? OutfitterGuidedHuntReport { get; set; }

    [Column($"{nameof(HuntedActivity)}_{nameof(SpecialGuidedHuntReportId)}")]
    public int? SpecialGuidedHuntReportId { get; set; }
    public SpecialGuidedHuntReport? SpecialGuidedHuntReport { get; set; }

    public override string GetAreaName(Report report) => GameManagementArea.ToString();
}

public class HuntedActivityConfig : IEntityTypeConfiguration<HuntedActivity>
{
    public void Configure(EntityTypeBuilder<HuntedActivity> builder) =>
        builder.ToTable(TableNameConstants.Activities);
}
