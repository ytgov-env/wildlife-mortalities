using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class SelectHarvestReportTypeComponent
    : ReportTypeComponent<SelectHarvestReportTypeViewModel>
{
    protected override void FieldsChanged()
    {
        if (_viewModel.HarvestReportType.HasValue == true)
        {
            HarvestReportTypeChanged.InvokeAsync(_viewModel.HarvestReportType.Value);
        }
    }

    [Parameter]
    public EventCallback<HarvestReportType> HarvestReportTypeChanged { get; set; }
}
