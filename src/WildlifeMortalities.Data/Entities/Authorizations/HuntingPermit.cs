using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingPermit : Authorization
{
    public PermitType Type { get; set; }

    public enum PermitType
    {
        Uninitialized = 0,
        CaribouFortymileFall,
        CaribouFortymileWinter,
        CaribouHartRiver,
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

    public bool IsCaribouRelated() => Type is PermitType.CaribouFortymileFall or PermitType.CaribouFortymileWinter
        or PermitType.CaribouHartRiver or PermitType.CaribouHartRiver or PermitType.CaribouNelchina or PermitType.CaribouSummer;
    public bool IsElkRelated() => Type is PermitType.Elk or PermitType.ElkAdaptive or PermitType.ElkAdaptiveFirstNation
        or PermitType.ElkAgricultural or PermitType.ElkBonusDraw;
    public bool IsMooseRelated() => Type is PermitType.Moose;
    public bool IsWoodBisonRelated() => Type is PermitType.WoodBison;

    public override AuthorizationResult IsValid(MortalityReport report)
    {
        if (report is HuntedMortalityReport huntedMortalityReport == false)
        {
            return AuthorizationResult.NotApplicable(this);
        }

        if (IsElkRelated() == true && huntedMortalityReport.Mortality is ElkMortality)
        {
            if (huntedMortalityReport.GameManagementArea.Subzone != "12")
            {
                return AuthorizationResult.Allowed(this);
            }
            else
            {
                return AuthorizationResult.Forbidden(this, "because it is not allowed!");
            }
        }

        return AuthorizationResult.NotApplicable(this);
    }
}

public class HuntingPermitConfig : IEntityTypeConfiguration<HuntingPermit>
{
    public void Configure(EntityTypeBuilder<HuntingPermit> builder)
    {
        builder
            .ToTable("Authorizations")
            .HasOne(s => s.BigGameHuntingLicence)
            .WithMany(h => h.HuntingPermits)
            .HasForeignKey(h => h.BigGameHuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
