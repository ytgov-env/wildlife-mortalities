using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class SelectMortalityReportTypeComponent
    : ReportTypeComponent<SelectMortalityReportTypeViewModel>
{
    protected override void FieldsChanged()
    {
        MortalityReportTypeChanged.InvokeAsync(ViewModel.MortalityReportType);
    }

    [Parameter]
    public EventCallback<MortalityReportType> MortalityReportTypeChanged { get; set; }
}
