using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class ResearchMortalityReport : MortalityReport { }

public class ResearchMortalityReportConfig : IEntityTypeConfiguration<ResearchMortalityReport>
{
    public void Configure(EntityTypeBuilder<ResearchMortalityReport> builder) =>
        builder.ToTable("Reports");
}
