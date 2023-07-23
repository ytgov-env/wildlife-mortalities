using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public class HumanWildlifeConflictMortalityReport
    : Report,
        IMultipleMortalitiesReport,
        IHasConservationOfficerReporter
{
    public List<HumanWildlifeConflictActivity> ConflictActivities { get; set; } = null!;

    [Column($"{nameof(HumanWildlifeConflictMortalityReport)}_{nameof(ConservationOfficerId)}")]
    public int ConservationOfficerId { get; set; }
    public ConservationOfficer ConservationOfficer { get; set; } = null!;

    [Column(
        $"{nameof(HumanWildlifeConflictMortalityReport)}_{nameof(HumanWildlifeConflictNumber)}"
    )]
    public string HumanWildlifeConflictNumber { get; set; } = string.Empty;

    [NotMapped]
    public override GeneralizedReportType GeneralizedReportType =>
        GeneralizedReportType.HumanWildlifeConflict;

    IEnumerable<Mortality> IMultipleMortalitiesReport.GetMortalities()
    {
        if (ConflictActivities == null)
        {
            return Enumerable.Empty<Mortality>();
        }

        return ConflictActivities.Select(x => x.Mortality).ToArray();
    }

    IEnumerable<Activity> IMultipleMortalitiesReport.GetActivities() =>
        ConflictActivities?.ToArray() ?? Array.Empty<HumanWildlifeConflictActivity>();

    public override bool HasHuntingActivity() => false;

    public override PersonWithAuthorizations GetPerson()
    {
        throw new Exception("This report type cannot have a PersonWithAuthorizations");
    }

    public override void OverrideActivity(IDictionary<Activity, Activity> replacements)
    {
        ConflictActivities = ConflictActivities.ConvertAll(
            x =>
                replacements.TryGetValue(x, out var activity)
                    ? (HumanWildlifeConflictActivity)activity
                    : x
        );
    }
}

public class HumanWildlifeConflictMortalityReportConfig
    : IEntityTypeConfiguration<HumanWildlifeConflictMortalityReport>
{
    public void Configure(EntityTypeBuilder<HumanWildlifeConflictMortalityReport> builder) =>
        builder
            .ToTable(TableNameConstants.Reports)
            .HasOne(h => h.ConservationOfficer)
            .WithMany(co => co.HumanWildlifeConflictReports)
            .OnDelete(DeleteBehavior.NoAction);
}
