using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class HumanWildlifeConflictMortalityReport : Report, ISingleMortalityReport
{
    [JsonConverter(typeof(MostConcreteClassJsonConverter<Mortality>))]
    public Mortality Mortality { get; set; } = null!;

    public int ConservationOfficerId { get; set; }
    public ConservationOfficer ConservationOfficer { get; set; } = null!;
    public string HumanWildlifeConflictNumber { get; set; } = string.Empty;

    public Mortality GetMortality() => Mortality;

    public Activity GetActivity() => throw new NotImplementedException();

    public override string GetHumanReadableIdPrefix() => "HWC";

    public override bool HasHuntingActivity() => false;
}

public class HumanWildlifeConflictMortalityReportConfig
    : IEntityTypeConfiguration<HumanWildlifeConflictMortalityReport>
{
    public void Configure(EntityTypeBuilder<HumanWildlifeConflictMortalityReport> builder) =>
        builder
            .ToTable("Reports")
            .HasOne(c => c.ConservationOfficer)
            .WithMany(co => co.HumanWildlifeConflictReports)
            .OnDelete(DeleteBehavior.NoAction);
}
