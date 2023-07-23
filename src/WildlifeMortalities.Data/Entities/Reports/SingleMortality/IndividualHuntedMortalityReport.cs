using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class IndividualHuntedMortalityReport : Report, ISingleMortalityReport
{
    public HuntedActivity Activity { get; set; } = null!;

    [Column($"{nameof(IndividualHuntedMortalityReport)}_{nameof(PersonId)}")]
    public int PersonId { get; set; }
    public PersonWithAuthorizations Person { get; set; } = null!;

    public Mortality GetMortality() => Activity.Mortality;

    public Activity GetActivity() => Activity;

    public override bool HasHuntingActivity() => true;

    [NotMapped]
    public override GeneralizedReportType GeneralizedReportType => GeneralizedReportType.Hunted;

    public override PersonWithAuthorizations GetPerson()
    {
        return Person;
    }

    public override void OverrideActivity(IDictionary<Activity, Activity> replacements)
    {
        if (replacements.TryGetValue(Activity, out var activity))
        {
            Activity = (HuntedActivity)activity;
        }
    }
}

public class IndividualHuntedMortalityReportConfig
    : IEntityTypeConfiguration<IndividualHuntedMortalityReport>
{
    public void Configure(EntityTypeBuilder<IndividualHuntedMortalityReport> builder)
    {
        builder.ToTable(TableNameConstants.Reports);
        builder.HasOne(h => h.Activity).WithOne(i => i.IndividualHuntedMortalityReport);
    }
}
