using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using System.ComponentModel.DataAnnotations.Schema;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class HumanWildlifeConflictActivity : Activity
{
    [Column($"{nameof(HumanWildlifeConflictActivity)}_{nameof(ReportId)}")]
    public int ReportId { get; set; }
    public HumanWildlifeConflictMortalityReport Report { get; set; } = null!;
}

public class HumanWildlifeConflictActivityConfig
    : IEntityTypeConfiguration<HumanWildlifeConflictActivity>
{
    public void Configure(EntityTypeBuilder<HumanWildlifeConflictActivity> builder) =>
        builder
            .ToTable(TableNameConstants.Activities)
            .HasOne(x => x.Report)
            .WithMany(x => x.ConflictActivities)
            .OnDelete(DeleteBehavior.NoAction);
}
