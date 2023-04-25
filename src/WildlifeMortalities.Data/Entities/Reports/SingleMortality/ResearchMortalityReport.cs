using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class ResearchMortalityReport : Report, ISingleMortalityReport
{
    public ResearchActivity Activity { get; set; } = null!;

    public Mortality GetMortality() => Activity.Mortality;

    public Activity GetActivity() => Activity;

    public override bool HasHuntingActivity() => false;
}

public class ResearchMortalityReportConfig : IEntityTypeConfiguration<ResearchMortalityReport>
{
    public void Configure(EntityTypeBuilder<ResearchMortalityReport> builder) =>
        builder.ToTable("Reports");
}
