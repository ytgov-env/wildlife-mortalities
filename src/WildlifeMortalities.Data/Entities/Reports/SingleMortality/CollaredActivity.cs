using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class CollaredActivity : Activity
{
    public int ReportId { get; set; }
    public CollaredMortalityReport Report { get; set; } = null!;
}

public class CollaredActivityConfig : IEntityTypeConfiguration<CollaredActivity>
{
    public void Configure(EntityTypeBuilder<CollaredActivity> builder) =>
        builder
            .ToTable("Activities")
            .HasOne(x => x.Report)
            .WithOne(x => x.Activity)
            .OnDelete(DeleteBehavior.NoAction);
}
