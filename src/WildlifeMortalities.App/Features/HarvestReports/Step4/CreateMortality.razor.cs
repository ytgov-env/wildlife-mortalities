using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports
{
    public partial class CreateMortality
    {
        [Parameter]
        public AllSpecies Species { get; set; }
    }
}
