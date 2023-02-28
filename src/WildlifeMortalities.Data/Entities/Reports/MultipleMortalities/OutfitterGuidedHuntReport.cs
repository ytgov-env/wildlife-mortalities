using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public class OutfitterGuidedHuntReport : Report, IMultipleMortalitiesReport, IHasClientReporter
{
    public DateTime? HuntStartDate { get; set; }
    public DateTime? HuntEndDate { get; set; }
    public int ChiefGuideId { get; set; }
    public Client ChiefGuide { get; set; } = null!;
    public List<Client> AssistantGuides { get; set; } = null!;
    public OutfitterArea OutfitterArea { get; set; } = null!;
    public GuidedHuntResult Result { get; set; }
    public List<HuntedActivity> HuntedActivities { get; set; } = null!;
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    IEnumerable<Mortality> IMultipleMortalitiesReport.GetMortalities()
    {
        if (HuntedActivities == null)
        {
            return Enumerable.Empty<Mortality>();
        }

        return HuntedActivities.Select(x => x.Mortality).ToArray();
    }

    IEnumerable<Activity> IMultipleMortalitiesReport.GetActivities()
    {
        if (HuntedActivities == null)
        {
            return Enumerable.Empty<HuntedActivity>();
        }

        return HuntedActivities.ToArray();
    }

    public override string GetHumanReadableIdPrefix() => "OGH";

    public override bool HasHuntingActivity() => true;

    public string GuidesToString()
    {
        var guides = string.Empty;
        foreach (var guide in AssistantGuides)
        {
            guides += $"{guide.FirstName} {guide.LastName} ({guide.EnvClientId}), ";
        }

        return guides[..^2];
    }
}

public class OutfitterGuidedHuntReportConfig : IEntityTypeConfiguration<OutfitterGuidedHuntReport>
{
    public void Configure(EntityTypeBuilder<OutfitterGuidedHuntReport> builder)
    {
        builder
            .ToTable("Reports")
            .HasOne(t => t.Client)
            .WithMany(t => t.OutfitterGuidedHuntReportsAsClient)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(o => o.ChiefGuide)
            .WithMany(c => c.OutfitterGuidedHuntReportsAsChiefGuide)
            .OnDelete(DeleteBehavior.NoAction);
        //builder
        //    .HasMany(o => o.AssistantGuides)
        //    .WithMany(c => c.OutfitterGuidedHuntReportsAsAssistantGuide);
    }
}
