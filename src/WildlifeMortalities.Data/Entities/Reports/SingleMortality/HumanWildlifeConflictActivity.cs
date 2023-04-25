using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class HumanWildlifeConflictActivity : Activity
{
    public int ReportId { get; set; }
    public HumanWildlifeConflictMortalityReport Report { get; set; } = null!;
}

public class HumanWildlifeConflictActivityConfig
    : IEntityTypeConfiguration<HumanWildlifeConflictActivity>
{
    public void Configure(EntityTypeBuilder<HumanWildlifeConflictActivity> builder) =>
        builder.ToTable("Activities");
}
