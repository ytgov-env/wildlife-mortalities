using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class CollaredMortalityReport : Report, ISingleMortalityReport
{
    [JsonConverter(typeof(MostConcreteClassJsonConverter<Mortality>))]
    public Mortality Mortality { get; set; } = null!;

    public string CollarNumber { get; set; } = string.Empty;

    public Mortality GetMortality() => Mortality;

    public Activity GetActivity() => throw new NotImplementedException();

    public override string GetHumanReadableIdPrefix() => "CMR";

    public override bool HasHuntingActivity() => false;
}

public class CollaredMortalityReportConfig : IEntityTypeConfiguration<CollaredMortalityReport>
{
    public void Configure(EntityTypeBuilder<CollaredMortalityReport> builder) =>
        builder.ToTable("Reports");
}
