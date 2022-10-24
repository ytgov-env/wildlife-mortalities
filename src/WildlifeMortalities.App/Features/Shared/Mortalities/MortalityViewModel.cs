using FluentValidation;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public class MortalityViewModel
{
    private static readonly Dictionary<AllSpecies, Func<Mortality>> s_mortalityFactory =
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

    public MortalityViewModel()
    {
    }

    public MortalityViewModel(Mortality mortality)
    {
        DateOfDeath = mortality.DateOfDeath;
        Latitude = mortality.Latitude;
        Longitude = mortality.Longitude;
        Sex = mortality.Sex;
    }

    public MortalityViewModel(AllSpecies species) => Species = species;

    public AllSpecies? Species { get; init; }

    public DateTime? DateOfDeath { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
    public Sex? Sex { get; set; }

    public virtual Dictionary<string, string> GetProperties()
    {
        var result = new Dictionary<string, string>
        {
            { "Species", Species.GetDisplayName() },
            { "Date of death", DateOfDeath?.Date.ToString() },
            { "Sex", Sex.GetDisplayName() }
        };

        if (Longitude.HasValue)
        {
            result.Add("Longitude", Longitude.Value.ToString());
        }

        if (Latitude.HasValue)
        {
            result.Add("Latitude", Latitude.Value.ToString());
        }

        return result;
    }

    public virtual Mortality GetMortality()
    {
        var mortalityFactory = s_mortalityFactory[Species.Value];
        var mortality = mortalityFactory.Invoke();
        SetBaseValues(mortality);

        return mortality;
    }

    protected void SetBaseValues(Mortality derivatingMortality)
    {
        derivatingMortality.DateOfDeath = DateOfDeath.Value;
        derivatingMortality.Latitude = Latitude;
        derivatingMortality.Longitude = Longitude;
        derivatingMortality.Sex = Sex!.Value;
    }
}

public abstract class MortalityViewModelBaseValidator<T> : AbstractValidator<T>
    where T : MortalityViewModel
{
    protected MortalityViewModelBaseValidator()
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

public class MortalityViewModelValidator : MortalityViewModelBaseValidator<MortalityViewModel>
{
}
