using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class MortalityComponent
{
    private AllSpecies? _currentSpecies;
    private MortalityViewModel? _editViewModel;

    [Parameter] [EditorRequired] public MortalityReportType? ReportType { get; set; }

    [Parameter]
    [EditorRequired]
    public MortalityViewModel? EditViewModel
    {
        get => _editViewModel;
        set
        {
            _editViewModel = value;
            if (value != null)
            {
                SetViewModel(value, false);
            }
        }
    }

    public MortalityViewModel GetViewModel() => ViewModel;

    private void SpeciesChanged(AllSpecies value)
    {
        if (_currentSpecies != value)
        {
            _currentSpecies = value;

            var viewModel = new MortalityViewModel(value);

            switch (_currentSpecies)
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

            SetViewModel(viewModel, true);
        }
    }
}
