using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class OutfitterChiefGuideLicence : Authorization
{
    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;
    public override AuthorizationResult IsValid(MortalityReport report) => throw new NotImplementedException();
}

public class OutfitterChiefGuideLicenceConfig : IEntityTypeConfiguration<OutfitterChiefGuideLicence>
{
    public void Configure(EntityTypeBuilder<OutfitterChiefGuideLicence> builder)
    {
        builder
            .ToTable("Authorizations")
            .HasOne(p => p.BigGameHuntingLicence)
            .WithOne(h => h.OutfitterChiefGuideLicence)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
