using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class IndividualHuntedMortalityReport : Report, ISingleMortalityReport
{
    public HuntedActivity HuntedActivity { get; set; } = null!;

    [Column($"{nameof(IndividualHuntedMortalityReport)}_{nameof(PersonId)}")]
    public int PersonId { get; set; }
    public PersonWithAuthorizations Person { get; set; } = null!;

    public Mortality GetMortality() => HuntedActivity.Mortality;

    public Activity GetActivity() => HuntedActivity;

    public override bool HasHuntingActivity() => true;

    [NotMapped]
    public override GeneralizedReportType GeneralizedReportType => GeneralizedReportType.Hunted;

    internal override PersonWithAuthorizations GetPerson()
    {
        return Person;
    }
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
