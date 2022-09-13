using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class SelectHarvestReportTypeComponent
    : ReportTypeComponent<SelectHarvestReportTypeViewModel>
{
    protected override void FieldsChanged()
    {
        HarvestReportTypeChanged.InvokeAsync(ViewModel.HarvestReportType);
    }

    [Parameter]
    public EventCallback<HarvestReportType> HarvestReportTypeChanged { get; set; }
}
