using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class IndividualHuntedMortalityReport : Report, ISingleMortalityReport, IHasClientReporter
{
    public int Id { get; set; }
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset DateCompleted { get; set; }
    public HuntedActivity HuntedActivity { get; set; } = null!;

    // Todo add status resolution logic
    public HuntedMortalityReportStatus Status { get; set; } = HuntedMortalityReportStatus.Complete;
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public Mortality GetMortality() => HuntedActivity.Mortality;

    public override string GetHumanReadableIdPrefix() => "HHR";

    public override bool HasHuntingActivity() => true;
}

public class IndividualHuntedMortalityReportConfig
    : IEntityTypeConfiguration<IndividualHuntedMortalityReport>
{
    public void Configure(EntityTypeBuilder<IndividualHuntedMortalityReport> builder)
    {
        builder.ToTable("Reports");
        builder.HasOne(h => h.HuntedActivity).WithOne(i => i.IndividualHuntedMortalityReport);
    }
}
