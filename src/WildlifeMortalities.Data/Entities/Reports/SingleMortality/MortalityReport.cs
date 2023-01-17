using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public abstract class MortalityReport : Report
{
    public Mortality Mortality { get; set; } = null!;

    public override IEnumerable<Mortality> GetMortalities() => new[] { Mortality };
}

public class MortalityReportConfig : IEntityTypeConfiguration<MortalityReport>
{
    public void Configure(EntityTypeBuilder<MortalityReport> builder) =>
        builder.HasOne(m => m.Mortality).WithOne(m => m.MortalityReport);
}
