using Microsoft.AspNetCore.Components;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class HarvestReportComponent : ReportTypeComponent<HarvestReportViewModel>
{
    [Parameter]
    public HarvestReportType ReportType { get; set; }
}
