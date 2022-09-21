using FluentValidation;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class MortalityViewModel
{
    public decimal? Longitute { get; set; }
    public decimal? Latitude { get; set; }
    public Sex? Sex { get; set; }
    public AllSpecies Species { get; }

    private static readonly Dictionary<AllSpecies, Func<Mortality>> _mortalityFactory =
        new()
        {
            { AllSpecies.AmericanBlackBear, () => new AmericanBlackBearMortality() },
            { AllSpecies.BarrenGroundCaribou, () => new BarrenGroundCaribouMortality() },
            { AllSpecies.Coyote, () => new CoyoteMortality() },
            { AllSpecies.Elk, () => new ElkMortality() },
            { AllSpecies.GreyWolf, () => new GreyWolfMortality() },
            { AllSpecies.GrizzlyBear, () => new GrizzlyBearMortality() },
            { AllSpecies.Moose, () => new MooseMortality() },
            { AllSpecies.MountainGoat, () => new MountainGoatMortality() },
            { AllSpecies.MuleDeer, () => new MuleDeerMortality() },
            { AllSpecies.ThinhornSheep, () => new ThinhornSheepMortality() },
            { AllSpecies.WhiteTailedDeer, () => new WhiteTailedDeerMortality() },
            { AllSpecies.Wolverine, () => new WolverineMortality() },
            { AllSpecies.WoodBison, () => new WoodBisonMortality() },
            { AllSpecies.WoodlandCaribou, () => new WoodlandCaribouMortality() }
        };

    public MortalityViewModel() { }

    public MortalityViewModel(Mortality mortality)
    {
        Latitude = mortality.Latitude;
        Longitute = mortality.Longitude;
        Sex = mortality.Sex;
    }

    public MortalityViewModel(AllSpecies species)
    {
        Species = species;
    }

    public virtual Mortality GetMortality()
    {
        var mortalityFactory = _mortalityFactory[Species];
        var mortality = mortalityFactory.Invoke();
        SetBaseValues(mortality);

        return mortality;
    }

    protected void SetBaseValues(Mortality derivatingMortality)
    {
        derivatingMortality.Latitude = Latitude;
        derivatingMortality.Longitude = Longitute;
        derivatingMortality.Sex = Sex!.Value;
    }
}

public class MortalityViewModelValidator : AbstractValidator<MortalityViewModel> {
    public MortalityViewModelValidator()
    {
        RuleFor(m => m.Sex).NotNull();
    }
}
