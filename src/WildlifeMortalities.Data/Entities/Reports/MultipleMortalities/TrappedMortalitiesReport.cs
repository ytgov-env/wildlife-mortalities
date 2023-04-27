using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public class TrappedMortalitiesReport : Report, IMultipleMortalitiesReport
{
    public List<TrappedActivity> TrappedActivities { get; set; } = null!;
    public int RegisteredTrappingConcessionId { get; set; }
    public RegisteredTrappingConcession RegisteredTrappingConcession { get; set; } = null!;
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    IEnumerable<Mortality> IMultipleMortalitiesReport.GetMortalities() =>
        TrappedActivities.Select(x => x.Mortality).ToArray();

    IEnumerable<Activity> IMultipleMortalitiesReport.GetActivities() => TrappedActivities.ToArray();

    public override bool HasHuntingActivity() => false;
}

public class TrappedMortalitiesReportConfig : IEntityTypeConfiguration<TrappedMortalitiesReport>
{
    public void Configure(EntityTypeBuilder<TrappedMortalitiesReport> builder) =>
        builder
            .ToTable("Reports")
            .HasOne(t => t.Client)
            .WithMany(t => t.TrappedMortalitiesReports)
            .OnDelete(DeleteBehavior.NoAction);
}
