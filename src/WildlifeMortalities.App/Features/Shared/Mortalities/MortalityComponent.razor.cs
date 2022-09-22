using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports
{
    public partial class MortalityComponent
    {
        private AllSpecies? _currentSpecies;

        [Parameter]
        [EditorRequired]
        public MortalityReportType ReportType { get; set; }

        private void SpeciesChanged(AllSpecies value)
        {
      
            if (_currentSpecies != value)
            {
                _currentSpecies = value;

                MortalityViewModel viewModel = new MortalityViewModel(value);

                switch (_currentSpecies)
                {
                    case AllSpecies.AmericanBlackBear:
                        viewModel = new AmericanBlackBearMortalityViewModel();
                        break;

                    case AllSpecies.WoodBison:
                        viewModel = new WoodBisonMortalityViewModel();
                        break;

                    default:
                        break;
                }

                SetViewModel(viewModel);
            }
        }
    }
}
