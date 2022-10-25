using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.MortalityReports;

public abstract class MortalityReport
{
    public int Id { get; set; }
    public Mortality Mortality { get; set; } = null!;
}

public class MortalityReportConfig : IEntityTypeConfiguration<MortalityReport>
{
    public void Configure(EntityTypeBuilder<MortalityReport> builder)
    {
        builder.HasOne(m => m.Mortality)
            .WithOne(m => m.MortalityReport);
    }
}
