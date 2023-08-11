using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Shared.Services.Reports.Single;

namespace WildlifeMortalities.App.Features.Reports;

public partial class MortalityReportComponent : DbContextAwareComponent
{
    private readonly IList<IBrowserFile> _files = new List<IBrowserFile>();
    private EditContext _editContext;
    private bool _invalidSubmitDetected;
    private int? _personId;
    private SignaturePadComponent _signaturePad = null!;
    private MortalityReportPageViewModel _vm;
    private bool _isSaving;
    private Client? _client;

    [Parameter]
    public int? DraftId { get; set; }

    [Parameter]
    public int? ReportId { get; set; }

    [Parameter]
    public string HumanReadablePersonId { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IMortalityService MortalityService { get; set; } = default!;

    [Inject]
    public ISnackbar SnackbarService { get; set; } = default!;

    [CascadingParameter]
    public AppParameters AppParameters { get; set; } = null!;

    private void CreateNewEditContext()
    {
        if (_editContext != null)
        {
            _editContext.OnFieldChanged -= EditContext_OnFieldChanged;
        }

        _editContext = new EditContext(_vm);
        _editContext.OnFieldChanged += EditContext_OnFieldChanged;
    }

    protected override void OnInitialized()
    {
        if (DraftId == null && ReportId == null)
        {
            _vm = new MortalityReportPageViewModel();
            CreateNewEditContext();
        }

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        using var context = GetContext();

        _client = await context.People
            .OfType<Client>()
            .Where(c => c.EnvPersonId == HumanReadablePersonId)
            .SingleOrDefaultAsync();

        _personId = _client?.Id;

        _personId ??= await context.People
            .OfType<ConservationOfficer>()
            .Where(c => c.BadgeNumber == HumanReadablePersonId)
            .Select(x => x.Id)
            .SingleOrDefaultAsync();

        if (DraftId != null)
        {
            var draft = await context.DraftReports.FirstOrDefaultAsync(x => x.Id == DraftId.Value);
            if (draft == null)
            {
                return;
            }

            _vm = JsonSerializer.Deserialize<MortalityReportPageViewModel>(draft.SerializedData)!;
            CreateNewEditContext();
        }
        else if (ReportId != null)
        {
            var report = await context.Reports
                .WithEntireGraph()
                .FirstOrDefaultAsync(x => x.Id == ReportId.Value);
            if (report == null)
            {
                return;
            }

            _vm = new MortalityReportPageViewModel(
                new ReportDetail(report, Array.Empty<(int, BioSubmission)>())
            );
            CreateNewEditContext();
        }
    }

    private void EditContext_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        if (_invalidSubmitDetected == false)
        {
            return;
        }

        _editContext.Validate();
        StateHasChanged();
    }

    private void ReportTypeChanged(ReportType type)
    {
        CreateNewEditContext();
        _vm.ReportType = type;

        _vm.ReportViewModel = _vm.ReportType switch
        {
            ReportType.IndividualHuntedMortalityReport
                => new IndividualHuntedMortalityReportViewModel(),
            ReportType.OutfitterGuidedHuntReport => new OutfitterGuidedHuntReportViewModel(),
            ReportType.SpecialGuidedHuntReport => new SpecialGuidedHuntReportViewModel(),
            ReportType.TrappedMortalitiesReport => new TrappedReportViewModel(),
            _ => throw new NotImplementedException(),
        };
    }

    private async Task SubmitReport()
    {
        if (_isSaving)
        {
            return;
        }

        _isSaving = true;
        if (ReportId == null)
        {
            await CreateReport();
        }
        else
        {
            await UpdateReport();
        }
        _isSaving = false;
    }

    // Todo: should allow user to save as draft if exception thrown by rule engine
    private async Task CreateDraftReport()
    {
        if (_isSaving)
        {
            return;
        }

        _isSaving = true;
        if (_editContext.GetValidationMessages().Any())
        {
            var personId = _personId!.Value;
            var content = JsonSerializer.Serialize(_vm);

            if (content.Length > 4000)
            {
                SnackbarService.Add("Draft report is too large to save.", Severity.Error);
                return;
            }

            if (DraftId != null)
            {
                await MortalityService.UpdateDraftReport(content, (int)DraftId);
            }
            else
            {
                await MortalityService.CreateDraftReport(
                    _vm.ReportType.GetReportType(),
                    content,
                    personId
                );
            }
            NavigationManager.NavigateTo(
                Constants.Routes.GetClientOverviewPageLink(HumanReadablePersonId)
            );
        }
        _isSaving = false;
    }

    private async Task CreateReport()
    {
        var personId = _personId!.Value;
        var report = _vm.ReportViewModel.GetReport(personId);
        Log.Information("Creating report");
        await MortalityService.CreateReport(report, AppParameters.UserId, DraftId);
        Log.Information("Created report");
        NavigationManager.NavigateTo(
            Constants.Routes.GetReportDetailsPageLink(HumanReadablePersonId, report.Id)
        );
    }

    private async Task UpdateReport()
    {
        var personId = _personId!.Value;
        var report = _vm.ReportViewModel.GetReport(personId);
        Log.Information("Updating report");
        await MortalityService.UpdateReport(report, AppParameters.UserId);
        Log.Information("Updated report");
        NavigationManager.NavigateTo(
            Constants.Routes.GetReportDetailsPageLink(HumanReadablePersonId, report.Id)
        );
    }

    private void UploadFiles(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            _files.Add(file);
        }
        //TODO upload the files to the server
    }
}
