using FluentValidation;
using WildlifeMortalities.App.Features.Shared.Mortalities.AmericanBlackBear;
using WildlifeMortalities.App.Features.Shared.Mortalities.Caribou;
using WildlifeMortalities.App.Features.Shared.Mortalities.Elk;
using WildlifeMortalities.App.Features.Shared.Mortalities.GrizzlyBear;
using WildlifeMortalities.App.Features.Shared.Mortalities.ThinhornSheep;
using WildlifeMortalities.App.Features.Shared.Mortalities.WoodBison;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Extensions;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public class MortalityViewModel
{
    private static readonly Dictionary<Species, Func<Mortality>> s_mortalityFactory = new();

    private readonly Mortality? _existingMortality;

    // This parameterless constructor is required for serializing this model when creating draft reports.
    public MortalityViewModel() { }

    static MortalityViewModel()
    {
        var mortalityType = typeof(Mortality);
        var relevantAssembly = mortalityType.Assembly;
        foreach (var item in relevantAssembly.GetTypes())
        {
            if (!item.IsSubclassOf(mortalityType))
            {
                continue;
            }

            var defaultConstructor = item.GetConstructor(Array.Empty<Type>());
            if (defaultConstructor == null)
            {
                continue;
            }

            var instance = (Mortality)defaultConstructor.Invoke(null);
            var species = instance.Species;

            s_mortalityFactory.Add(species, () => (Mortality)defaultConstructor.Invoke(null));
        }
    }

    public MortalityViewModel(Mortality mortality, ReportDetail? reportDetail = null)
    {
        DateOfDeath = mortality.DateOfDeath?.DateTime;
        Latitude = mortality.Latitude;
        Longitude = mortality.Longitude;
        Sex = mortality.Sex;
        Species = mortality.Species;

        _existingMortality = mortality;
        Id = mortality.Id;
        if (reportDetail != null)
        {
            BioSubmission = reportDetail.BioSubmissions
                .FirstOrDefault(x => x.mortalityId == mortality.Id)
                .submission;
        }
    }

    public MortalityViewModel(Species species) => Species = species;

    public BioSubmission? BioSubmission { get; set; }

    public int? Id { get; private set; }
    public Species? Species { get; init; }
    public DateTime? DateOfDeath { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
    public Sex? Sex { get; set; }

    public bool IsDraft { get; set; }

    public virtual Dictionary<string, string?> GetProperties()
    {
        var result = new Dictionary<string, string?> { { "Sex", Sex?.GetDisplayName() } };

        if (DateOfDeath.HasValue)
        {
            result.Add("Date of death", DateOfDeath.Value.Date.ToString());
        }

        if (Latitude.HasValue)
        {
            result.Add("Latitude", Latitude.Value.ToString());
        }

        if (Longitude.HasValue)
        {
            result.Add("Longitude", Longitude.Value.ToString());
        }

        return result;
    }

    private Mortality GetMortality(Species species)
    {
        var mortality = _existingMortality;
        if (mortality == null)
        {
            var mortalityFactory = s_mortalityFactory[species];
            mortality = mortalityFactory.Invoke();
        }

        SetBaseValues(mortality);

        return mortality;
    }

    public virtual Mortality GetMortality() => GetMortality(Species!.Value);

    protected void SetBaseValues(Mortality derivatingMortality)
    {
        derivatingMortality.Id = Id ?? 0;
        derivatingMortality.DateOfDeath =
            DateOfDeath == null
                ? null
                : new DateTimeOffset((DateTime)DateOfDeath, TimeSpan.FromHours(-7));
        derivatingMortality.Latitude = Latitude;
        derivatingMortality.Longitude = Longitude;
        derivatingMortality.Sex = Sex;
    }

    internal static MortalityViewModel Create(Mortality mortality, ReportDetail? reportDetail) =>
        Create(null, mortality, reportDetail);

    internal static MortalityViewModel Create(Species value, MortalityViewModel? previous)
    {
        var result = Create(value, null, null);
        if (previous != null)
        {
            result.DateOfDeath = previous.DateOfDeath;
            result.Latitude = previous.Latitude;
            result.Longitude = previous.Longitude;
            result.Sex = previous.Sex;
        }

        return result;
    }

    private static MortalityViewModel Create(
        Species? value,
        Mortality? mortality,
        ReportDetail? reportDetail
    )
    {
        var species =
            (mortality?.Species ?? value)
            ?? throw new ArgumentException("Either species or mortality needs to be specified");

        return species switch
        {
            Data.Enums.Species.AmericanBlackBear
                => mortality == null
                    ? new AmericanBlackBearMortalityViewModel()
                    : new AmericanBlackBearMortalityViewModel(
                        (AmericanBlackBearMortality)mortality,
                        reportDetail
                    ),
            Data.Enums.Species.Caribou
                => mortality == null
                    ? new CaribouMortalityViewModel()
                    : new CaribouMortalityViewModel((CaribouMortality)mortality, reportDetail),
            Data.Enums.Species.Elk
                => mortality == null
                    ? new ElkMortalityViewModel()
                    : new ElkMortalityViewModel((ElkMortality)mortality, reportDetail),
            Data.Enums.Species.GrizzlyBear
                => mortality == null
                    ? new GrizzlyBearMortalityViewModel()
                    : new GrizzlyBearMortalityViewModel(
                        (GrizzlyBearMortality)mortality,
                        reportDetail
                    ),
            Data.Enums.Species.ThinhornSheep
                => mortality == null
                    ? new ThinhornSheepMortalityViewModel()
                    : new ThinhornSheepMortalityViewModel(
                        (ThinhornSheepMortality)mortality,
                        reportDetail
                    ),
            Data.Enums.Species.WoodBison
                => mortality == null
                    ? new WoodBisonMortalityViewModel()
                    : new WoodBisonMortalityViewModel((WoodBisonMortality)mortality, reportDetail),
            _
                => mortality == null
                    ? new MortalityViewModel(species)
                    : new MortalityViewModel(mortality, reportDetail),
        };
    }
}

public abstract class MortalityViewModelBaseValidator<T> : AbstractValidator<T>
    where T : MortalityViewModel
{
    protected MortalityViewModelBaseValidator()
    {
        RuleFor(m => m.Latitude)
            .Must(latitude => latitude is null or >= 58 and <= 71)
            .WithMessage("Latitude must be between 58 and 71");
        //RuleFor(m => m.Latitude).PrecisionScale(5, 3, true).When(x => x.Latitude is not null);
        RuleFor(m => m.Latitude)
            .Null()
            .When(m => m.Longitude is null)
            .WithMessage("Latitude cannot be set when longitude is null.");
        RuleFor(m => m.Latitude)
            .NotNull()
            .When(m => m.Longitude is not null)
            .WithMessage("Latitude must be set if longitude is set.");

        RuleFor(m => m.Longitude)
            .Must(longitude => longitude is null or >= -143 and <= -121)
            .WithMessage("Longitude must be between -121 and -143");
        //RuleFor(m => m.Longitude).PrecisionScale(6, 3, true).When(x => x.Longitude is not null);

        RuleFor(m => m.Longitude)
            .Null()
            .When(m => m.Latitude is null)
            .WithMessage("Longitude cannot be set when latitude is null.");
        RuleFor(m => m.Longitude)
            .NotNull()
            .When(m => m.Latitude is not null)
            .WithMessage("Longitude must be set if latitude is set.");

        RuleFor(m => m.Sex)
            .NotNull()
            .IsInEnum()
            .WithMessage("Sex must be set to female, male, or unknown.");
        RuleFor(m => m.DateOfDeath).NotNull().WithMessage("Please select a date of death.");
        RuleFor(m => m.DateOfDeath)
            .Must(x => x <= DateTime.Now)
            .When(m => m.DateOfDeath is not null)
            .WithMessage("The date of death cannot occur in the future.");
    }
}

public class MortalityViewModelValidator : MortalityViewModelBaseValidator<MortalityViewModel> { }
