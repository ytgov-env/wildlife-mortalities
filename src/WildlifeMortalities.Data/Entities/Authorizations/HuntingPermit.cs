using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingPermit : Authorization
{
    public enum PermitType
    {
        [Display(Name = "Fortymile caribou summer")]
        CaribouFortymileSummer = 10,

        [Display(Name = "Fortymile caribou winter")]
        CaribouFortymileWinter = 20,

        [Display(Name = "Hart river caribou")]
        CaribouHartRiver = 30,

        [Display(Name = "Nelchina caribou")]
        CaribouNelchina = 40,

        [Display(Name = "Elk adaptive")]
        ElkAdaptive = 50,

        [Display(Name = "Elk adaptive first nations")]
        ElkAdaptiveFirstNations = 60,

        [Display(Name = "Elk exclusion")]
        ElkExclusion = 70,

        [Display(Name = "Moose threshold")]
        MooseThreshold = 80,

        [Display(Name = "Bison threshold")]
        WoodBisonThreshold = 90
    }

    public HuntingPermit() { }

    public HuntingPermit(PermitType type) => Type = type;

    public PermitType Type { get; set; }

    public bool IsCaribouRelated() =>
        Type
            is PermitType.CaribouFortymileSummer
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
        builder.ToTable("Authorizations");
}
