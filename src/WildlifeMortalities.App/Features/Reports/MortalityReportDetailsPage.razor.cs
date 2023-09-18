using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Shared.Services.Files;
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
    public string HumanReadablePersonId { get; set; } = null!;

    [CascadingParameter]
    public AppParameters AppParameters { get; set; } = null!;

    private Client? Client { get; set; }

    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    [Inject]
    public IMortalityService MortalityService { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        using var context = GetContext();
        _reportDetail = await context.Reports.GetDetails(ReportId, context);
        Client = await context.People
            .OfType<Client>()
            .SingleOrDefaultAsync(x => x.EnvPersonId == HumanReadablePersonId);
        _isLoading = false;
    }

    private ActivityViewModel GetActivityVm(Activity item) =>
        item switch
        {
            HuntedActivity activity => new HuntedActivityViewModel(activity, _reportDetail),
            TrappedActivity activity => new TrappedActivityViewModel(activity, _reportDetail),
            _ => throw new Exception($"no viewmodel mapping found for type {item.GetType().Name}")
        };

    private string GetCreatedBy()
    {
        if (_reportDetail is null)
        {
            return string.Empty;
        }
        return $"{_reportDetail.Report.CreatedBy.FullName} on {_reportDetail.Report.DateCreated.ToString(Constants.FormatStrings.StandardDateFormat)}";
    }

    private string? GetLastModifiedBy()
    {
        if (_reportDetail is null)
        {
            return null;
        }

        BioSubmission? bioSubmission = null;
        if (_reportDetail.BioSubmissions.Any())
        {
            bioSubmission = _reportDetail.BioSubmissions
                .MaxBy(x => x.submission.DateModified)
                .submission;
        }

        var result = string.Empty;
        if (
            (
                _reportDetail.Report.LastModifiedBy is not null
                && bioSubmission?.LastModifiedBy is null
            )
            || (
                bioSubmission is not null
                && _reportDetail.Report.DateModified > bioSubmission.DateModified
                && _reportDetail.Report.LastModifiedBy is not null
            )
        )
        {
            result =
                $"{_reportDetail.Report.LastModifiedBy.FullName} on {_reportDetail.Report.DateModified?.ToString(Constants.FormatStrings.StandardDateFormat)}";
        }
        else if (bioSubmission?.LastModifiedBy is not null)
        {
            result =
                $"{bioSubmission.LastModifiedBy.FullName} on {bioSubmission.DateModified?.ToString(Constants.FormatStrings.StandardDateFormat)}";
        }
        else
        {
            result = null;
        }

        return result != GetCreatedBy() ? result : null;
    }

    private async Task Delete()
    {
        var parameters = new DialogParameters
        {
            [nameof(DeleteHarvestReportDialog.HumanReadableId)] = _reportDetail!
                .Report
                .HumanReadableId
        };

        var dialog = DialogService.Show<DeleteHarvestReportDialog>("", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            var reportViewModel = new MortalityReportPageViewModel(_reportDetail);

            var report = JsonSerializer.Serialize(reportViewModel);
            await MortalityService.SoftDeleteReport(
                report,
                _reportDetail.Report.Id,
                AppParameters.UserId,
                (string)result.Data
            );
            NavigationManager.NavigateTo(Constants.Routes.ReportsOverviewPage);
        }
    }
}
