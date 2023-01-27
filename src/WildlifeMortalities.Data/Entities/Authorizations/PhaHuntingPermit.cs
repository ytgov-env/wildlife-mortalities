using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class PhaHuntingPermit : Authorization
{
    public enum PermitType
    {
        Caribou = 10,
        Deer = 20,
        Elk = 30,
        MountainGoat = 40,
        Moose = 50,
        ThinhornSheep = 60,
        ThinhornSheepKluane = 70
    }

    public PhaHuntingPermit()
    {
    }

    public PhaHuntingPermit(PermitType type) => Type = type;

    public PermitType Type { get; set; }

    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();
}

public class PhaHuntingPermitConfig : IEntityTypeConfiguration<PhaHuntingPermit>
{
    public void Configure(EntityTypeBuilder<PhaHuntingPermit> builder) =>
        builder
            .ToTable("Authorizations")
            .HasOne(p => p.BigGameHuntingLicence)
            .WithMany(h => h.PhaHuntingPermits)
            .HasForeignKey(p => p.BigGameHuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);
}
