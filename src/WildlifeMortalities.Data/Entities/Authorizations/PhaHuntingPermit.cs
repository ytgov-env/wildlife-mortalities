using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class PhaHuntingPermit : Authorization, IHasBigGameHuntingLicence
{
    public enum PermitType
    {
        [Display(Name = "Caribou")]
        Caribou = 10,

        [Display(Name = "Deer")]
        Deer = 20,

        [Display(Name = "Elk")]
        Elk = 30,

        [Display(Name = "Goat")]
        MountainGoat = 40,

        [Display(Name = "Moose")]
        Moose = 50,

        [Display(Name = "Sheep")]
        ThinhornSheep = 60,

        [Display(Name = "Kluane sheep")]
        ThinhornSheepKluane = 70
    }

    public PhaHuntingPermit() { }

    public PhaHuntingPermit(PermitType type) => Type = type;

    public PermitType Type { get; set; }
    public string HuntCode { get; set; } = null!;

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
