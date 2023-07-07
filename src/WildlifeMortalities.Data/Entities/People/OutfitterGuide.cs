using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

namespace WildlifeMortalities.Data.Entities.People;

public class OutfitterGuide
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? ReportAsChiefGuideId { get; set; }
    public OutfitterGuidedHuntReport? ReportAsChiefGuide { get; set; }
    public int? ReportAsAssistantGuideId { get; set; }
    public OutfitterGuidedHuntReport? ReportAsAssistantGuide { get; set; }
}

public class OutfitterGuideConfig : IEntityTypeConfiguration<OutfitterGuide>
{
    public void Configure(EntityTypeBuilder<OutfitterGuide> builder) =>
        builder.ToTable(Constants.TableNameConstants.OutfitterGuides);
}
