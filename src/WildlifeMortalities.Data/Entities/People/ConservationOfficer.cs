using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

namespace WildlifeMortalities.Data.Entities.People;

public class ConservationOfficer : Person
{
    public string BadgeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public List<HumanWildlifeConflictMortalityReport> HumanWildlifeConflictReports { get; set; } =
        null!;
}

public class ConservationOfficerConfig : IEntityTypeConfiguration<ConservationOfficer>
{
    public void Configure(EntityTypeBuilder<ConservationOfficer> builder)
    {
        builder.HasIndex(c => c.BadgeNumber).IsUnique();
        builder.Property(c => c.FirstName).HasColumnName(nameof(ConservationOfficer.FirstName));
        builder.Property(c => c.LastName).HasColumnName(nameof(ConservationOfficer.LastName));
    }
}
