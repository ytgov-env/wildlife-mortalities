using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public class SpecialGuidedHuntReport : Report, IMultipleMortalitiesReport, IHasClientReporter
{
    public DateTime HuntStartDate { get; set; }
    public DateTime HuntEndDate { get; set; }
    public int GuideId { get; set; }
    public Client Guide { get; set; } = null!;
    public GuidedHuntResult Result { get; set; }
    public List<HuntedActivity> HuntedActivities { get; set; } = null!;
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    IEnumerable<Mortality> IMultipleMortalitiesReport.GetMortalities() =>
        HuntedActivities.Select(x => x.Mortality).ToArray();

    IEnumerable<Activity> IMultipleMortalitiesReport.GetActivities() => HuntedActivities.ToArray();

    public override string GetHumanReadableIdPrefix() => "SGH";

    public override bool HasHuntingActivity() => true;
}

public class SpecialGuidedHuntReportConfig : IEntityTypeConfiguration<SpecialGuidedHuntReport>
{
    public void Configure(EntityTypeBuilder<SpecialGuidedHuntReport> builder)
    {
        builder
            .ToTable("Reports")
            .HasOne(m => m.Guide)
            .WithMany(m => m.SpecialGuidedHuntReportsAsGuide)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(t => t.Client)
            .WithMany(t => t.SpecialGuidedHuntReportsAsClient)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
