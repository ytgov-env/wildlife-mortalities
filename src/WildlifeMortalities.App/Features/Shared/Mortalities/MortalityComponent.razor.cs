using Microsoft.AspNetCore.Components;
using WildlifeMortalities.App.Features.MortalityReports;
using WildlifeMortalities.App.Features.Shared.Mortalities.AmericanBlackBear;
using WildlifeMortalities.App.Features.Shared.Mortalities.GrizzlyBear;
using WildlifeMortalities.App.Features.Shared.Mortalities.ThinhornSheep;
using WildlifeMortalities.App.Features.Shared.Mortalities.WoodBison;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class MortalityComponent
{
    [Parameter]
    [EditorRequired]
    public MortalityReportType ReportType { get; set; }

    [Parameter]
    public bool DisableSpeciesSelection { get; set; } = false;

    [Parameter]
    [EditorRequired]
    public MortalityWithSpeciesSelectionViewModel ViewModel { get; set; } = null!;

    public MortalityViewModel GetViewModel() => ViewModel.MortalityViewModel;

    private void SpeciesChanged(Species? value)
    {
        if (value.HasValue == false)
        {
            return;
            ;
        }

        if (ViewModel.Species == value)
        {
            return;
        }

        ViewModel.Species = value;

        var viewModel = new MortalityViewModel(value.Value);

        switch (value)
        {
            case Species.AmericanBlackBear:
                viewModel = new AmericanBlackBearMortalityViewModel();
                break;

            case Species.GrizzlyBear:
                viewModel = new GrizzlyBearMortalityViewModel();
                break;

            case Species.ThinhornSheep:
                viewModel = new ThinhornSheepMortalityViewModel();
                break;

            case Species.WoodBison:
                viewModel = new WoodBisonMortalityViewModel();
                break;
        }

        ViewModel.MortalityViewModel = viewModel;
    }
}
