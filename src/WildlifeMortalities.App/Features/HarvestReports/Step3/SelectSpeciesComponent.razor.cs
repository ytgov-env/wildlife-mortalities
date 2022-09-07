using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class SelectSpeciesComponent
{
    //private Dictionary<HuntedSpecies, Func<Mortality>> mortalityFactory = new Dictionary<HuntedSpecies, Func<Mortality>>
    //    {
    //        { HuntedSpecies.AmericanBlackBear, () => new AmericanBlackBearMortality()  },
    //    };

    //public T CreateMortality<T>(HuntedSpecies species) where T : Mortality
    //{
    //    var mortality = mortalityFactory[species]();
    //    return (T)mortality;
    //}

    [Parameter]
    public HarvestReportType ReportType { get; set; }

    private static readonly Dictionary<TrappedSpecies, AllSpecies> _trappedMapper = new()
    {
        {TrappedSpecies.GreyWolf, AllSpecies.GreyWolf },
        { TrappedSpecies.Wolverine, AllSpecies.Wolverine }
    };

    private static readonly Dictionary<HuntedSpecies, AllSpecies> _huntedMapper = new()
    {
        { HuntedSpecies.Wolverine, AllSpecies.Wolverine },
        { HuntedSpecies.AmericanBlackBear, AllSpecies.AmericanBlackBear },
    };

    protected override void FieldsChanged()
    {
        if (_viewModel.HuntedSpecies.HasValue)
        {
            SpeciesChanged.InvokeAsync(_huntedMapper[_viewModel.HuntedSpecies.Value]);
        }
        else if (_viewModel.TrappedSpecies.HasValue)
        {
            SpeciesChanged.InvokeAsync(_trappedMapper[_viewModel.TrappedSpecies.Value]);
        }
    }

    [Parameter]
    public EventCallback<AllSpecies> SpeciesChanged { get; set; }
}
