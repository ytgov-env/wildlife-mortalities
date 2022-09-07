using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class SelectHarvestReportTypeComponent
{
    [Parameter]
    public EventCallback<bool> ValidationChanged { get; set; }

    private SelectHarvestReportTypeViewModel _selectHarvestReportTypeViewModel = null!;

    protected override void OnInitialized() => _selectHarvestReportTypeViewModel = new();
}
