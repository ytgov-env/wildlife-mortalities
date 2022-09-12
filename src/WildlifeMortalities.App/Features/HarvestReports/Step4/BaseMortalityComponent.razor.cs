using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class BaseMortalityComponent
{

    [Parameter] public CreateMortalityViewModel ViewModel { get; set; }
}
