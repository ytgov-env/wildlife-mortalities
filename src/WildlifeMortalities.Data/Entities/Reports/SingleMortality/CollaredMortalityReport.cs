using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class CollaredMortalityReport : Report, ISingleMortalityReport
{
    public CollaredActivity CollaredActivity { get; set; } = null!;
    public string CollarNumber { get; set; } = string.Empty;

    public Mortality GetMortality() => CollaredActivity.Mortality;

    public Activity GetActivity() => CollaredActivity;

    public override bool HasHuntingActivity() => false;
}

public class CollaredMortalityReportConfig : IEntityTypeConfiguration<CollaredMortalityReport>
{
    public void Configure(EntityTypeBuilder<CollaredMortalityReport> builder) =>
        builder.ToTable("Reports");
}
