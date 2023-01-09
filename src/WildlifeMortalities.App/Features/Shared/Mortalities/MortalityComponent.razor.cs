using Microsoft.AspNetCore.Components;
using WildlifeMortalities.App.Features.MortalityReports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class MortalityComponent
{
    [Parameter] [EditorRequired] public MortalityReportType ReportType { get; set; }
    [Parameter] public bool DisableSpeciesSelection { get; set; } = false;

    [Parameter]
    [EditorRequired]
    public MortalityWithSpeciesSelectionViewModel ViewModel { get; set; } = null!;

    public MortalityViewModel GetViewModel() => ViewModel.MortalityViewModel;

    private void SpeciesChanged(AllSpecies? value)
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
            case AllSpecies.AmericanBlackBear:
                viewModel = new AmericanBlackBearMortalityViewModel();
                break;

            case AllSpecies.GrizzlyBear:
                viewModel = new GrizzlyBearMortalityViewModel();
                break;

            case AllSpecies.ThinhornSheep:
                viewModel = new ThinhornSheepMortalityViewModel();
                break;

            case AllSpecies.WoodBison:
                viewModel = new WoodBisonMortalityViewModel();
                break;
        }

        ViewModel.MortalityViewModel = viewModel;
    }
}
