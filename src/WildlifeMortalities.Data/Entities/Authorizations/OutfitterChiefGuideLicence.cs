using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class OutfitterChiefGuideLicence : Authorization
{
    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;
}

public class OutfitterChiefGuideLicenceConfig : IEntityTypeConfiguration<OutfitterChiefGuideLicence>
{
    public void Configure(EntityTypeBuilder<OutfitterChiefGuideLicence> builder)
    {
        builder.HasOne(p => p.BigGameHuntingLicence)
            .WithOne(h => h.OutfitterChiefGuideLicence)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
