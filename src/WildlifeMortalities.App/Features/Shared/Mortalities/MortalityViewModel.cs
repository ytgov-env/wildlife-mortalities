using FluentValidation;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public class MortalityViewModel
{
    private static readonly Dictionary<Species, Func<Mortality>> s_mortalityFactory =
        new()
        {
            { Data.Enums.Species.AmericanBlackBear, () => new AmericanBlackBearMortality() },
            { Data.Enums.Species.Caribou, () => new CaribouMortality() },
            { Data.Enums.Species.Coyote, () => new CoyoteMortality() },
            { Data.Enums.Species.Elk, () => new ElkMortality() },
            { Data.Enums.Species.GreyWolf, () => new GreyWolfMortality() },
            { Data.Enums.Species.GrizzlyBear, () => new GrizzlyBearMortality() },
            { Data.Enums.Species.Moose, () => new MooseMortality() },
            { Data.Enums.Species.MountainGoat, () => new MountainGoatMortality() },
            { Data.Enums.Species.MuleDeer, () => new MuleDeerMortality() },
            { Data.Enums.Species.ThinhornSheep, () => new ThinhornSheepMortality() },
            { Data.Enums.Species.WhiteTailedDeer, () => new WhiteTailedDeerMortality() },
            { Data.Enums.Species.Wolverine, () => new WolverineMortality() },
            { Data.Enums.Species.WoodBison, () => new WoodBisonMortality() }
        };

    private readonly Mortality? _existingMortality;

    public MortalityViewModel()
    {
    }

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

        if (
            reportDetail?.report
            is not null
            and IndividualHuntedMortalityReport individualHuntedMortalityReport
        )
        {
            Landmark = individualHuntedMortalityReport.HuntedActivity.Landmark;
            Comment = individualHuntedMortalityReport.HuntedActivity.Comment;
        }
    }

    public MortalityViewModel(Species species) => Species = species;

    public BioSubmission? ExistingBioSubmission { get; }

    public int? Id { get; }
    public Species? Species { get; init; }
    public DateTime? DateOfDeath { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
    public string? Landmark { get; set; }
    public string? Comment { get; set; }
    public Sex? Sex { get; set; }

    public virtual Dictionary<string, string> GetProperties()
    {
        var result = new Dictionary<string, string> { { "Sex", Sex!.GetDisplayName() } };

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
        RuleFor(m => m.DateOfDeath).NotNull().WithMessage("Please select a date of death");
    }
}

public class MortalityViewModelValidator : MortalityViewModelBaseValidator<MortalityViewModel>
{
}
