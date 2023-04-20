using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class IndividualHuntedMortalityReport : Report, ISingleMortalityReport, IHasClientReporter
{
    public HuntedActivity HuntedActivity { get; set; } = null!;
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public Mortality GetMortality() => HuntedActivity.Mortality;

    public Activity GetActivity() => HuntedActivity;

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
