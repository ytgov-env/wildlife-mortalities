using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports
{
    public partial class MortalityComponent
    {
        private AllSpecies _species;

        [Parameter]
        public AllSpecies Species { get; set; }

        protected override void OnParametersSet()
        {
            if (_species != Species)
            {
                _species = Species;
                MortalityViewModel viewModel = new MortalityViewModel(Species);

                switch (Species)
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

            base.OnParametersSet();
        }
    }
}
