using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class BaseMortalityComponent
{
    [Parameter]
    public MortalityViewModel ViewModel { get; set; } = null!;
}
