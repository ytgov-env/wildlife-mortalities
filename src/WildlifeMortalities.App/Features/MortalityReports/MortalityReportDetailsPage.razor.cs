using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class MortalityReportDetailsPage
{
    private ReportDetail? _reportDetail;

    [Inject] private IMortalityService MortalityService { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        _reportDetail = await MortalityService.GetReport(Id);
}
