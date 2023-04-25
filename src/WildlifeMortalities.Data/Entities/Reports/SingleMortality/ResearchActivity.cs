using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class ResearchActivity : Activity
{
    public int ReportId { get; set; }
    public ResearchMortalityReport Report { get; set; } = null!;
}

public class ResearchActivityConfig : IEntityTypeConfiguration<ResearchActivity>
{
    public void Configure(EntityTypeBuilder<ResearchActivity> builder) =>
        builder.ToTable("Activities");
}
