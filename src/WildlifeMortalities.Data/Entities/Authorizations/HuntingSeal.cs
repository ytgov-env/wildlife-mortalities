using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingSeal : Authorization, IHasBigGameHuntingLicence
{
    public enum SealType
    {
        AmericanBlackBear = 10,
        Caribou = 20,
        Deer = 30,
        Elk = 40,
        GrizzlyBear = 50,
        Moose = 60,
        MountainGoat = 70,
        ThinhornSheep = 80,
        WoodBison = 90
    }

    public HuntingSeal() { }

    public HuntingSeal(SealType type) => Type = type;

    public SealType Type { get; set; }
    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = null!;
    public HuntedActivity? HuntedMortalityReport { get; set; }

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();
}

public class SealConfig : IEntityTypeConfiguration<HuntingSeal>
{
    public void Configure(EntityTypeBuilder<HuntingSeal> builder) =>
        builder
            .ToTable("Authorizations")
            .HasOne(s => s.BigGameHuntingLicence)
            .WithMany(h => h.HuntingSeals)
            .HasForeignKey(s => s.BigGameHuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);
}
