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
        CaribouFortymileSummer,
        CaribouFortymileWinter,
        CaribouHartRiver,
        CaribouNelchina,
        ElkExclusion,
        ElkAdaptive,
        ElkAdaptiveFirstNations,
        MooseThreshold,
        WoodBisonThreshold
    }

    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;

    public bool IsCaribouRelated() => Type is PermitType.CaribouFortymileFall or PermitType.CaribouFortymileWinter
        or PermitType.CaribouHartRiver or PermitType.CaribouHartRiver or PermitType.CaribouNelchina
        or PermitType.CaribouFortymileSummer;

    public bool IsElkRelated() =>
        Type is PermitType.ElkExclusion or PermitType.ElkAdaptive or PermitType.ElkAdaptiveFirstNations;

    public bool IsMooseRelated() => Type is PermitType.MooseThreshold;
    public bool IsWoodBisonRelated() => Type is PermitType.WoodBisonThreshold;

    public override AuthorizationResult GetResult(MortalityReport report)
    {
        if (report is HuntedMortalityReport huntedMortalityReport == false)
        {
            return AuthorizationResult.NotApplicable(this);
        }

        AuthorizationResult authorizationResult = new(this, new List<Violation>());

        // if (IsCaribouRelated() &&
        //     huntedMortalityReport.Mortality is WoodlandCaribouMortality or BarrenGroundCaribouMortality)
        // {
        // }
        // else if (IsElkRelated() && huntedMortalityReport.Mortality is ElkMortality)
        // {
        // }
        // else if (IsMooseRelated() && huntedMortalityReport.Mortality is MooseMortality)
        // {
        // }
        // else if (IsWoodBisonRelated() && huntedMortalityReport.Mortality is WoodBisonMortality)
        // {
        // }
        // else
        // {
        //     return AuthorizationResult.NotApplicable(this);
        // }
        throw new NotImplementedException();
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
