using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

namespace WildlifeMortalities.Data.Entities.People;

public class ConservationOfficer : Person
{
    [Column($"{nameof(ConservationOfficer)}_{nameof(BadgeNumber)}")]
    public string BadgeNumber { get; set; } = string.Empty;

    [Column($"{nameof(ConservationOfficer)}_{nameof(FirstName)}")]
    public string FirstName { get; set; } = string.Empty;

    [Column($"{nameof(ConservationOfficer)}_{nameof(LastName)}")]
    public string LastName { get; set; } = string.Empty;

    public List<HumanWildlifeConflictMortalityReport> HumanWildlifeConflictReports { get; set; } =
        null!;
}

public class ConservationOfficerConfig : IEntityTypeConfiguration<ConservationOfficer>
{
    public void Configure(EntityTypeBuilder<ConservationOfficer> builder)
    {
        builder.HasIndex(c => c.BadgeNumber).IsUnique();
    }
}
