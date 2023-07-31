using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Shared.Services.Reports.Single;

namespace WildlifeMortalities.App.Features.Reports;

public partial class MortalityReportDetailsPage : DbContextAwareComponent
{
    private ReportDetail? _reportDetail;
    private bool _isLoading = true;

    [Inject]
    private PdfService PdfService { get; set; } = null!;

    [Parameter]
    public int ReportId { get; set; }

    [Parameter]
    public string HumanReadablePersonId { get; set; }

    private Client? Client { get; set; }

    protected override async Task OnInitializedAsync()
    {
        using var context = GetContext();
        _reportDetail = await context.Reports.GetDetails(ReportId, context);
        _isLoading = false;
        Client = await context.People
            .OfType<Client>()
            .SingleOrDefaultAsync(x => x.EnvPersonId == HumanReadablePersonId);
    }

    private ActivityViewModel GetActivityVm(Activity item) =>
        item switch
        {
            HuntedActivity activity => new HuntedActivityViewModel(activity, _reportDetail),
            TrappedActivity activity => new TrappedActivityViewModel(activity, _reportDetail),
            _ => throw new Exception($"no viewmodel mapping found for type {item.GetType().Name}")
        };
}
