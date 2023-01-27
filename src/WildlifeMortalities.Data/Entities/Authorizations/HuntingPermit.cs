using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingPermit : Authorization
{
    public enum PermitType
    {
        CaribouFortymileFall = 10,
        CaribouFortymileSummer = 20,
        CaribouFortymileWinter = 30,
        CaribouHartRiver = 40,
        CaribouNelchina = 50,
        ElkAdaptive = 60,
        ElkAdaptiveFirstNations = 70,
        ElkExclusion = 80,
        MooseThreshold = 90,
        WoodBisonThreshold = 100
    }

    public HuntingPermit()
    {
    }

    public HuntingPermit(PermitType type) => Type = type;

    public PermitType Type { get; set; }

    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;

    public bool IsCaribouRelated() =>
        Type
            is PermitType.CaribouFortymileFall
            or PermitType.CaribouFortymileSummer
            or PermitType.CaribouFortymileWinter
            or PermitType.CaribouHartRiver
            or PermitType.CaribouNelchina;

    public bool IsElkRelated() =>
        Type
            is PermitType.ElkExclusion
            or PermitType.ElkAdaptive
            or PermitType.ElkAdaptiveFirstNations;

    public bool IsMooseRelated() => Type is PermitType.MooseThreshold;

    public bool IsWoodBisonRelated() => Type is PermitType.WoodBisonThreshold;

    public override AuthorizationResult GetResult(Report report)
    {
        if (report.HasHuntingActivity() == false)
        {
            return AuthorizationResult.NotApplicable(this);
        }

        // AuthorizationResult authorizationResult = new(this, new List<Violation>());

        // if (IsCaribouRelated() &&
        //     huntedMortalityReport.Mortality is CaribouMortality)
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
    public void Configure(EntityTypeBuilder<HuntingPermit> builder) =>
        builder
            .ToTable("Authorizations")
            .HasOne(s => s.BigGameHuntingLicence)
            .WithMany(h => h.HuntingPermits)
            .HasForeignKey(h => h.BigGameHuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);
}
