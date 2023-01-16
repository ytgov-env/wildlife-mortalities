using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class CollaredMortalityReport : MortalityReport
{
    public string CollarNumber { get; set; } = string.Empty;
}

public class CollaredMortalityReportConfig : IEntityTypeConfiguration<CollaredMortalityReport>
{
    public void Configure(EntityTypeBuilder<CollaredMortalityReport> builder) =>
        builder.ToTable("Reports");
}
