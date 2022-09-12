using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports
{
    public partial class MortalityComponent
    {
        private MortalityViewModel _viewModel;

        [Parameter]
        public AllSpecies Species { get; set; }

        protected override void OnParametersSet()
        {
            _viewModel = new(Species);

            switch (Species)
            {
                case AllSpecies.AmericanBlackBear:
                    _viewModel = new AmericanBlackBearMortalityViewModel();
                    break;
                case AllSpecies.WoodBison:
                    _viewModel = new WoodBisonMortalityViewModel();
                    break;
                default:
                    break;
            }
            base.OnParametersSet();
        }

        private void CreateInstance()
        {
            var mortality =  _viewModel.GetMortality();
            Console.WriteLine(mortality.GetType().Name);

        }
    }
}
