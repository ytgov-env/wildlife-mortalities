using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class SpecialGuideLicence : Authorization
{
    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;
}

public class SpecialGuideLicenceConfig : IEntityTypeConfiguration<SpecialGuideLicence>
{
    public void Configure(EntityTypeBuilder<SpecialGuideLicence> builder)
    {
        builder
            .HasOne(s => s.BigGameHuntingLicence)
            .WithOne(h => h.SpecialGuideLicence)
            .HasForeignKey<SpecialGuideLicence>(s => s.BigGameHuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
