
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingPermit : Authorization
{
    public PermitType Type { get; set; }
    public enum PermitType
    {
        Uninitialized = 0,
        CaribouFortyMileFall,
        CaribouFortyMileWinter,
        CaribouHart,
        CaribouNelchina,
        CaribouSummer,
        Elk,
        ElkAdaptive,
        ElkAdaptiveFirstNation,
        ElkAgricultural,
        ElkBonusDraw,
        Moose,
        WoodBison
    }
    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;
}

public class HuntingPermitConfig : IEntityTypeConfiguration<HuntingPermit>
{
    public void Configure(EntityTypeBuilder<HuntingPermit> builder)
    {
        builder.HasOne(s => s.BigGameHuntingLicence)
            .WithMany(h => h.HuntingPermits)
            .HasForeignKey(h => h.BigGameHuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
