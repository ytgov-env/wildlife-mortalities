using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class PhaHuntingPermit : Authorization
{
    public PermitType Type { get; set; }
    public enum PermitType
    {
        Uninitialized = 0,
        Caribou,
        Deer,
        Elk,
        Goat,
        Moose,
        Sheep,
        SheepKluane
    }

    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;
}

public class PhaHuntingPermitConfig : IEntityTypeConfiguration<PhaHuntingPermit>
{
    public void Configure(EntityTypeBuilder<PhaHuntingPermit> builder)
    {
        builder.HasOne(p => p.BigGameHuntingLicence)
            .WithMany(h => h.PhaHuntingPermits)
            .HasForeignKey(p => p.BigGameHuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
