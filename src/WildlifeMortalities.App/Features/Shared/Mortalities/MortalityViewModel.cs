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
            { AllSpecies.Caribou, () => new CaribouMortality() },
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
            { AllSpecies.WoodBison, () => new WoodBisonMortality() }
        };

    public MortalityViewModel() { }

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
            { "Species", Species!.GetDisplayName() },
            { "Sex", Sex!.GetDisplayName() }
        };

        if (DateOfDeath.HasValue)
        {
            result.Add("Date of death", DateOfDeath.Value.Date.ToString());
        }

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

    public Mortality GetMortality(AllSpecies species)
    {
        var mortalityFactory = s_mortalityFactory[species];
        var mortality = mortalityFactory.Invoke();
        SetBaseValues(mortality);

        return mortality;
    }

    public virtual Mortality GetMortality() => GetMortality(Species!.Value);

    protected void SetBaseValues(Mortality derivatingMortality)
    {
        derivatingMortality.DateOfDeath = DateOfDeath;
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
        RuleFor(m => m.Latitude)
            .Must(latitude => latitude is null or > 58 and < 71)
            .WithMessage("Latitude must be between 58 and 71");
        RuleFor(m => m.Latitude)
            .Null()
            .When(m => m.Longitude is null)
            .WithMessage("Latitude cannot be set when longitude is null");
        RuleFor(m => m.Latitude)
            .NotNull()
            .When(m => m.Longitude is not null)
            .WithMessage("Latitude must be set if longitude is set");

        RuleFor(m => m.Longitude)
            .Must(longitude => longitude is null or > -143 and < -121)
            .WithMessage("Longitude must be between -121 and -143");
        RuleFor(m => m.Longitude)
            .Null()
            .When(m => m.Latitude is null)
            .WithMessage("Longitude cannot be set when latitude is null");
        RuleFor(m => m.Longitude)
            .NotNull()
            .When(m => m.Latitude is not null)
            .WithMessage("Longitude must be set if latitude is set");

        RuleFor(m => m.Sex)
            .NotNull()
            .IsInEnum()
            .WithMessage("Sex must be set to Female, Male, or Unknown");
        // TODO Is DateOfDeath required?
        RuleFor(m => m.DateOfDeath)
            .NotNull()
            .WithMessage("Please select a date of death");
    }
}

public class MortalityViewModelValidator : MortalityViewModelBaseValidator<MortalityViewModel> { }
