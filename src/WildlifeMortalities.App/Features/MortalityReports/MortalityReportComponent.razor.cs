using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class MortalityReportComponent : ReportTypeComponent<MortalityReportViewModel>
{
    [Parameter]
    public MortalityReportType ReportType { get; set; }
}
