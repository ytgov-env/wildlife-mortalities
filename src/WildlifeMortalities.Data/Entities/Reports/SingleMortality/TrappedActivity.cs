using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class TrappedActivity : HarvestActivity
{
    [Column($"{nameof(TrappedActivity)}_{nameof(TrappedMortalitiesReportId)}")]
    public int TrappedMortalitiesReportId { get; set; }
    public TrappedMortalitiesReport TrappedMortalitiesReport { get; set; } = null!;
}

public class TrappedActivityConfig : IEntityTypeConfiguration<TrappedActivity>
{
    public void Configure(EntityTypeBuilder<TrappedActivity> builder) =>
        builder
            .ToTable(TableNameConstants.Activities)
            .HasOne(t => t.TrappedMortalitiesReport)
            .WithMany(t => t.TrappedActivities)
            .OnDelete(DeleteBehavior.NoAction);
}
