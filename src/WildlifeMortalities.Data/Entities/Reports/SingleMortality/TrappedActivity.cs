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

    public HarvestMethodType HarvestMethod { get; set; }

    public override string GetAreaName(Report report) =>
        (report as TrappedMortalitiesReport)?.RegisteredTrappingConcession.Area
        ?? "<error invalid concession>";

    public enum HarvestMethodType
    {
        LegholdTrap = 10,
        ConibearTrap = 20,
        NeckSnare = 30,

        //Todo: discrete set of other values?
        Other = 40
    }
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
