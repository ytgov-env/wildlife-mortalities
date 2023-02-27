using FluentValidation;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public class MortalityViewModel
{
    private static readonly Dictionary<Species, Func<Mortality>> s_mortalityFactory = new();

    private readonly Mortality? _existingMortality;

    static MortalityViewModel()
    {
        var mortalityType = typeof(Mortality);
        var relevantAssembly = mortalityType.Assembly;
        var allTypes = relevantAssembly.GetTypes();

        foreach (var item in allTypes)
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

    public MortalityViewModel() { }

    public MortalityViewModel(Mortality mortality, ReportDetail? reportDetail = null)
    {
        DateOfDeath = mortality.DateOfDeath;
        Latitude = mortality.Latitude;
        Longitude = mortality.Longitude;
        Sex = mortality.Sex;
        Species = mortality.Species;

        _existingMortality = mortality;
        Id = mortality.Id;
        ExistingBioSubmission = reportDetail.bioSubmissions
            .FirstOrDefault(x => x.mortalityId == mortality.Id)
            .submission;
    }

    public MortalityViewModel(Species species) => Species = species;

    public BioSubmission? ExistingBioSubmission { get; }

    public int? Id { get; }
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
        if (_existingMortality != null)
        {
            return _existingMortality;
        }

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
        derivatingMortality.Sex = Sex;
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
            .WithMessage("Latitude cannot be set when longitude is null.");
        RuleFor(m => m.Latitude)
            .NotNull()
            .When(m => m.Longitude is not null)
            .WithMessage("Latitude must be set if longitude is set.");

        RuleFor(m => m.Longitude)
            .Must(longitude => longitude is null or > -143 and < -121)
            .WithMessage("Longitude must be between -121 and -143");
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
            .WithMessage("Sex must be set to Female, Male, or Unknown.");
        RuleFor(m => m.DateOfDeath).NotNull().WithMessage("Please select a date of death.");
    }
}

public class MortalityViewModelValidator : MortalityViewModelBaseValidator<MortalityViewModel> { }
