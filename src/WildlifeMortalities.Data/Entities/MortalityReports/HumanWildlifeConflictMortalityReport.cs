using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.MortalityReports;

public class HumanWildlifeConflictMortalityReport : MortalityReport
{
    public int ConservationOfficerId { get; set; }
    public ConservationOfficer ConservationOfficer { get; set; } = null!;
    public string HumanWildlifeConflictNumber { get; set; } = string.Empty;
}

public class HumanWildlifeConflictMortalityReportConfig
    : IEntityTypeConfiguration<HumanWildlifeConflictMortalityReport>
{
    public void Configure(EntityTypeBuilder<HumanWildlifeConflictMortalityReport> builder) =>
        builder
            .HasOne(c => c.ConservationOfficer)
            .WithMany(co => co.HumanWildlifeConflictReports)
            .OnDelete(DeleteBehavior.NoAction);
}
