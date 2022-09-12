using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports
{
    public partial class CreateMortality
    {
        private CreateMortalityViewModel _viewModel;

        [Parameter]
        public AllSpecies Species { get; set; }

        protected override void OnParametersSet()
        {
            _viewModel = new(Species);

            switch (Species)
            {
                case AllSpecies.AmericanBlackBear:
                    _viewModel = new CreateAmericanBlackBearMortalityViewModel();
                    break;
                case AllSpecies.WoodBison:
                    _viewModel = new CreateWoodBisonMortalityViewModel();
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
