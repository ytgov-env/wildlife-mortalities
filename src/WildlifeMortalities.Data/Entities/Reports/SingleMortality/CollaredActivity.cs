using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class CollaredActivity : Activity
{
    [Column($"{nameof(CollaredActivity)}_{nameof(ReportId)}")]
    public int ReportId { get; set; }
    public CollaredMortalityReport Report { get; set; } = null!;
}

public class CollaredActivityConfig : IEntityTypeConfiguration<CollaredActivity>
{
    public void Configure(EntityTypeBuilder<CollaredActivity> builder) =>
        builder
            .ToTable(TableNameConstants.Activities)
            .HasOne(x => x.Report)
            .WithOne(x => x.Activity)
            .OnDelete(DeleteBehavior.NoAction);
}
