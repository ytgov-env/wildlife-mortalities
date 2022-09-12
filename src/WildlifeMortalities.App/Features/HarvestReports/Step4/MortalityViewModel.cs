using FluentValidation;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports;

public class MortalityViewModel
{
    public decimal? Longitute { get; set; }
    public decimal? Latitude { get; set; }
    public Sex Sex { get; set; }
    public AllSpecies Species { get; }

    private static Dictionary<AllSpecies, Func<Mortality>> _mortalityFactory = new()
    {
        { AllSpecies.Elk, () => new ElkMortality() }
    };

    public MortalityViewModel()
    {

    }

    public MortalityViewModel(Mortality mortality)
    {
        Latitude = mortality.Latitude;
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
        derivatingMortality.Sex = Sex;
    }

}

public class MortalityViewModelValidator : AbstractValidator<MortalityViewModel> { }
