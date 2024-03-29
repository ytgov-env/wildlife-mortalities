﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public class OutfitterGuidedHuntReport : Report, IMultipleMortalitiesReport
{
    [Column($"{nameof(OutfitterGuidedHuntReport)}_{nameof(HuntStartDate)}")]
    public DateTime? HuntStartDate { get; set; }

    [Column($"{nameof(OutfitterGuidedHuntReport)}_{nameof(HuntEndDate)}")]
    public DateTime? HuntEndDate { get; set; }

    public OutfitterGuide? ChiefGuide { get; set; }
    public List<OutfitterGuide> AssistantGuides { get; set; } = null!;

    [Column($"{nameof(OutfitterGuidedHuntReport)}_{nameof(OutfitterAreaId)}")]
    public int OutfitterAreaId { get; set; }
    public OutfitterArea OutfitterArea { get; set; } = null!;

    [Column($"{nameof(OutfitterGuidedHuntReport)}_{nameof(Result)}")]
    public GuidedHuntResult Result { get; set; }
    public List<HuntedActivity> HuntedActivities { get; set; } = null!;

    [Column($"{nameof(OutfitterGuidedHuntReport)}_{nameof(OheNumber)}")]
    public string OheNumber { get; set; } = string.Empty;

    [Column($"{nameof(OutfitterGuidedHuntReport)}_{nameof(ClientId)}")]
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

    public string GuidesToString()
    {
        var guides = string.Empty;
        foreach (var guide in AssistantGuides)
        {
            guides += $"{guide.FirstName} {guide.LastName}, ";
        }

        return guides[..^2];
    }

    public override PersonWithAuthorizations GetPerson()
    {
        return Client;
    }

    public override void OverrideActivity(IDictionary<Activity, Activity> replacements)
    {
        if (HuntedActivities == null)
        {
            return;
        }
        HuntedActivities = HuntedActivities.ConvertAll(
            x => replacements.TryGetValue(x, out var activity) ? (HuntedActivity)activity : x
        );
    }
}

public class OutfitterGuidedHuntReportConfig : IEntityTypeConfiguration<OutfitterGuidedHuntReport>
{
    public void Configure(EntityTypeBuilder<OutfitterGuidedHuntReport> builder)
    {
        builder
            .ToTable(TableNameConstants.Reports)
            .HasOne(t => t.Client)
            .WithMany(t => t.OutfitterGuidedHuntReportsAsClient)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(o => o.ChiefGuide)
            .WithOne(c => c.ReportAsChiefGuide)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasMany(o => o.AssistantGuides)
            .WithOne(c => c.ReportAsAssistantGuide)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
