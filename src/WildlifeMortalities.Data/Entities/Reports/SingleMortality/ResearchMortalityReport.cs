using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class ResearchMortalityReport : Report, ISingleMortalityReport
{
    public Mortality Mortality { get; set; }

    public Mortality GetMortality() => Mortality;

    public override string GetHumanReadableIdPrefix() => "RMR";

    public override bool HasHuntingActivity() => false;
}

public class ResearchMortalityReportConfig : IEntityTypeConfiguration<ResearchMortalityReport>
{
    public void Configure(EntityTypeBuilder<ResearchMortalityReport> builder) =>
        builder.ToTable("Reports");
}
