using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public class SpecialGuidedHuntReport : Report, IMultipleMortalitiesReport
{
    [Column($"{nameof(SpecialGuidedHuntReport)}_{nameof(HuntStartDate)}")]
    public DateTime? HuntStartDate { get; set; }

    [Column($"{nameof(SpecialGuidedHuntReport)}_{nameof(HuntEndDate)}")]
    public DateTime? HuntEndDate { get; set; }

    [Column($"{nameof(SpecialGuidedHuntReport)}_{nameof(GuideId)}")]
    public int GuideId { get; set; }
    public Client Guide { get; set; } = null!;

    [Column($"{nameof(SpecialGuidedHuntReport)}_{nameof(Result)}")]
    public GuidedHuntResult Result { get; set; }
    public List<HuntedActivity> HuntedActivities { get; set; } = null!;

    [Column($"{nameof(SpecialGuidedHuntReport)}_{nameof(ClientId)}")]
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    [NotMapped]
    public override GeneralizedReportType GeneralizedReportType => GeneralizedReportType.Hunted;

    IEnumerable<Mortality> IMultipleMortalitiesReport.GetMortalities()
    {
        if (HuntedActivities == null)
        {
            return Enumerable.Empty<Mortality>();
        }

        return HuntedActivities.Select(x => x.Mortality).ToArray();
    }

    IEnumerable<Activity> IMultipleMortalitiesReport.GetActivities() =>
        HuntedActivities?.ToArray() ?? Array.Empty<HuntedActivity>();

    public override bool HasHuntingActivity() => true;

    public override PersonWithAuthorizations GetPerson()
    {
        return Client;
    }

    public override void OverrideActivity(IDictionary<Activity, Activity> replacements)
    {
        HuntedActivities = HuntedActivities.ConvertAll(
            x => replacements.TryGetValue(x, out var activity) ? (HuntedActivity)activity : x
        );
    }
}

public class SpecialGuidedHuntReportConfig : IEntityTypeConfiguration<SpecialGuidedHuntReport>
{
    public void Configure(EntityTypeBuilder<SpecialGuidedHuntReport> builder)
    {
        builder
            .ToTable(TableNameConstants.Reports)
            .HasOne(m => m.Guide)
            .WithMany(m => m.SpecialGuidedHuntReportsAsGuide)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(t => t.Client)
            .WithMany(t => t.SpecialGuidedHuntReportsAsClient)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
