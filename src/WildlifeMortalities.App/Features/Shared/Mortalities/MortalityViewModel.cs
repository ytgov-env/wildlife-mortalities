using FluentValidation;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class MortalityViewModel
{
    public AllSpecies? Species { get; init; }

    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
    public Sex? Sex { get; set; }

    public virtual Dictionary<string, string> GetProperties()
    {
        var result = new Dictionary<string, string> { { "Species", Species.GetDisplayName() }, };

        if (Longitude.HasValue)
        {
            result.Add("Longitude", Longitude.Value.ToString());
        }

        if (Latitude.HasValue)
        {
            result.Add("Latitude", Latitude.Value.ToString());
        }

        result.Add("Sex", Sex.GetDisplayName());

        return result;
    }

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
        Longitude = mortality.Longitude;
        Sex = mortality.Sex;
    }

    public MortalityViewModel(AllSpecies species)
    {
        Species = species;
    }

    public virtual Mortality GetMortality()
    {
        var mortalityFactory = _mortalityFactory[Species.Value];
        var mortality = mortalityFactory.Invoke();
        SetBaseValues(mortality);

        return mortality;
    }

    protected void SetBaseValues(Mortality derivatingMortality)
    {
        derivatingMortality.Latitude = Latitude;
        derivatingMortality.Longitude = Longitude;
        derivatingMortality.Sex = Sex!.Value;
    }
}

public class MortalityViewModelValidator : AbstractValidator<MortalityViewModel>
{
    public MortalityViewModelValidator()
    {
        RuleFor(m => m.Sex).NotNull();
        RuleFor(m => m.Latitude)
            .Must(latitude => latitude is null || (latitude > 58 && latitude < 71))
            .WithMessage("Latitude must be between 58°N and 71°N");
        RuleFor(m => m.Latitude)
            .Null()
            .When(m => m.Longitude is null)
            .WithMessage("Latitude cannot be set when longitude is null");

        RuleFor(m => m.Longitude)
            .Must(longitude => longitude is null || (longitude > -143 && longitude < -121))
            .WithMessage("Longitude must be between 121°W and 143°W");
        RuleFor(m => m.Longitude)
            .Null()
            .When(m => m.Latitude is null)
            .WithMessage("Longitude cannot be set when latitude is null");
    }
}
