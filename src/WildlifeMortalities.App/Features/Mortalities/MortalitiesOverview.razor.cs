using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Mortalities;

public partial class MortalitiesOverview
{
    private List<Mortality> _mortalities = new();

    [Inject] private MortalityService MortalityService { get; set; }

    [Inject] private NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync() => _mortalities = await MortalityService.GetAllMortalities();

    private void ReportAHarvest() => NavigationManager.NavigateTo("mortality-reports/create");
}
