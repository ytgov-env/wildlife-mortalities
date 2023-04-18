using Microsoft.AspNetCore.Components;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Shared.Services.Reports.Single;

namespace WildlifeMortalities.App.Features.Reports;

public partial class MortalityReportDetailsPage : DbContextAwareComponent
{
    private ReportDetail? _reportDetail;

    [Inject]
    private PdfService PdfService { get; set; } = null!;

    [Parameter]
    public int ReportId { get; set; }

    [Parameter]
    public string HumanReadablePersonId { get; set; }

    protected override async Task OnInitializedAsync() =>
        _reportDetail = await Context.Reports.GetDetails(ReportId, Context);

    private ActivityViewModel GetActivityVm(Activity item) =>
        item switch
        {
            HuntedActivity activity => new HuntedActivityViewModel(activity, _reportDetail),
            TrappedActivity activity => new TrappedActivityViewModel(activity, _reportDetail),
            _ => throw new Exception($"no viewmodel mapping found for type {item.GetType().Name}")
        };

    private bool HasIncompleteBioSubmissions()
    {
        var mortalitiesThatRequireABioSubmission = _reportDetail.report
            .GetMortalities()
            .OfType<IHasBioSubmission>()
            .Count();
        return mortalitiesThatRequireABioSubmission
            != _reportDetail.bioSubmissions.Count(
                x => x.submission.Status == BioSubmissionStatus.AnalysisComplete
            );
    }
}
