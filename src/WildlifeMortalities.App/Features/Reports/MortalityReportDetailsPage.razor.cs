using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public partial class MortalityReportDetailsPage
{
    private ReportDetail? _reportDetail;

    [Inject]
    private IMortalityService MortalityService { get; set; } = null!;

    [Inject]
    private PdfService PdfService { get; set; } = null!;

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        _reportDetail = await MortalityService.GetReport(Id);

    private ActivityViewModel GetActivityVm(Activity item) =>
        item switch
        {
            HuntedActivity activity => new HuntedActivityViewModel(activity, _reportDetail),
            TrappedActivity activity => new TrappedActivityViewModel(activity, _reportDetail),
            _ => throw new Exception($"no viewmodel mapping found for type {item.GetType().Name}")
        };
}
