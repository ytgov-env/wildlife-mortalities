using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public class OutfitterGuidedHuntReport : Report, IMultipleMortalitiesReport, IHasClientReporter
{
    public DateTime HuntStartDate { get; set; }
    public DateTime HuntEndDate { get; set; }
    public List<Client> Guides { get; set; } = null!;
    public OutfitterArea OutfitterArea { get; set; } = null!;
    public GuidedHuntResult Result { get; set; }
    public List<HuntedActivity> HuntedActivities { get; set; } = null!;
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    IEnumerable<Mortality> IMultipleMortalitiesReport.GetMortalities() =>
        HuntedActivities.Select(x => x.Mortality).ToArray();

    public override string GetHumanReadableIdPrefix() => "HHR";

    public override bool HasHuntingActivity() => true;
}

public class OutfitterGuidedHuntReportConfig : IEntityTypeConfiguration<OutfitterGuidedHuntReport>
{
    public void Configure(EntityTypeBuilder<OutfitterGuidedHuntReport> builder) =>
        builder
            .ToTable("Reports")
            .HasOne(t => t.Client)
            .WithMany(t => t.OutfitterGuidedHuntReports)
            .OnDelete(DeleteBehavior.NoAction);
}
