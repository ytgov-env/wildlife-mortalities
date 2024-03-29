﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class CollaredMortalityReport : Report, ISingleMortalityReport
{
    public CollaredActivity Activity { get; set; } = null!;

    [Column($"{nameof(CollaredMortalityReport)}_{nameof(CollarNumber)}")]
    public string CollarNumber { get; set; } = string.Empty;

    public Mortality GetMortality() => Activity.Mortality;

    public Activity GetActivity() => Activity;

    public override bool HasHuntingActivity() => false;

    [NotMapped]
    public override GeneralizedReportType GeneralizedReportType => GeneralizedReportType.Collared;

    public override PersonWithAuthorizations GetPerson()
    {
        throw new Exception("This report type cannot have a PersonWithAuthorizations");
    }

    public override void OverrideActivity(IDictionary<Activity, Activity> replacements)
    {
        if (replacements.TryGetValue(Activity, out var activity))
        {
            Activity = (CollaredActivity)activity;
        }
    }
}

public class CollaredMortalityReportConfig : IEntityTypeConfiguration<CollaredMortalityReport>
{
    public void Configure(EntityTypeBuilder<CollaredMortalityReport> builder) =>
        builder.ToTable(TableNameConstants.Reports);
}
