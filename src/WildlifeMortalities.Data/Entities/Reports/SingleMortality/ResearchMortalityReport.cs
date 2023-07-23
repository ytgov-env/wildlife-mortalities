using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class ResearchMortalityReport : Report, ISingleMortalityReport
{
    public ResearchActivity Activity { get; set; } = null!;

    public Mortality GetMortality() => Activity.Mortality;

    public Activity GetActivity() => Activity;

    public override bool HasHuntingActivity() => false;

    [NotMapped]
    public override GeneralizedReportType GeneralizedReportType => GeneralizedReportType.Research;

    public override PersonWithAuthorizations GetPerson()
    {
        throw new Exception("This report type cannot have a PersonWithAuthorizations");
    }

    public override void OverrideActivity(IDictionary<Activity, Activity> replacements)
    {
        if (replacements.TryGetValue(Activity, out var activity))
        {
            Activity = (ResearchActivity)activity;
        }
    }
}

public class ResearchMortalityReportConfig : IEntityTypeConfiguration<ResearchMortalityReport>
{
    public void Configure(EntityTypeBuilder<ResearchMortalityReport> builder) =>
        builder.ToTable(TableNameConstants.Reports);
}
