using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingPermit : Authorization
{
    public enum PermitType
    {
        CaribouFortymileFall = 10,
        CaribouFortymileSummerPeriodOne = 20,
        CaribouFortymileSummerPeriodTwo = 30,
        CaribouFortymileSummerPeriodThree = 40,
        CaribouFortymileSummerPeriodFour = 50,
        CaribouFortymileSummerPeriodFive = 60,
        CaribouFortymileSummerPeriodSix = 70,
        CaribouFortymileSummerPeriodSeven = 80,
        CaribouFortymileSummerPeriodEight = 90,
        CaribouFortymileWinter = 100,
        CaribouHartRiver = 110,
        CaribouNelchina = 120,
        ElkAdaptive = 130,
        ElkAdaptiveFirstNations = 140,
        ElkExclusion = 150,
        MooseThreshold = 160,
        WoodBisonThreshold = 170
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
            or PermitType.CaribouFortymileSummerPeriodOne
            or PermitType.CaribouFortymileSummerPeriodTwo
            or PermitType.CaribouFortymileSummerPeriodThree
            or PermitType.CaribouFortymileSummerPeriodFour
            or PermitType.CaribouFortymileSummerPeriodFive
            or PermitType.CaribouFortymileSummerPeriodSix
            or PermitType.CaribouFortymileSummerPeriodSeven
            or PermitType.CaribouFortymileSummerPeriodEight
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

    public override AuthorizationResult GetResult(MortalityReport report)
    {
        if (report is HuntedMortalityReport huntedMortalityReport == false)
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
