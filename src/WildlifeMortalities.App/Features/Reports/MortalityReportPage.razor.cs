using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Shared.Services.Reports.Single;

namespace WildlifeMortalities.App.Features.Reports;

public partial class MortalityReportPage : DbContextAwareComponent
{
    private readonly IList<IBrowserFile> _files = new List<IBrowserFile>();
    private EditContext _editContext;
    private bool _invalidSubmitDetected;
    private int? _personId;
    private SignaturePadComponent _signaturePad = null!;
    private MortalityReportPageViewModel _vm;

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
        _personId = await Context.People
            .OfType<Client>()
            .Where(c => c.EnvClientId == HumanReadablePersonId)
            .Select(x => x.Id)
            .SingleOrDefaultAsync();

        _personId ??= await Context.People
            .OfType<ConservationOfficer>()
            .Where(c => c.BadgeNumber == HumanReadablePersonId)
            .Select(x => x.Id)
            .SingleOrDefaultAsync();

        if (DraftId != null)
        {
            var draft = await Context.DraftReports.FirstOrDefaultAsync(x => x.Id == DraftId.Value);
            if (draft == null)
            {
                return;
            }

            _vm = JsonSerializer.Deserialize<MortalityReportPageViewModel>(draft.SerializedData)!;
            CreateNewEditContext();
        }
        else if (ReportId != null)
        {
            var report = await Context.Reports
                .WithEntireGraph()
                .FirstOrDefaultAsync(x => x.Id == ReportId.Value);
            if (report == null)
            {
                return;
            }

            _vm = new MortalityReportPageViewModel(report);
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

        switch (_vm.ReportType)
        {
            case ReportType.IndividualHuntedMortalityReport:
                _vm.ReportViewModel = new IndividualHuntedMortalityReportViewModel();
                ;
                break;
            case ReportType.OutfitterGuidedHuntReport:
                _vm.ReportViewModel = new OutfitterGuidedHuntReportViewModel();
                break;
            case ReportType.SpecialGuidedHuntReport:
                _vm.ReportViewModel = new SpecialGuidedHuntReportViewModel();
                break;
            case ReportType.TrappedMortalitiesReport:
                _vm.ReportViewModel = new TrappedReportViewModel();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private async Task SubmitReport()
    {
        if (ReportId == null)
        {
            await CreateReport();
        }
        else
        {
            await UpdateReport();
        }
    }

    private async Task CreateDraftReport()
    {
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
    }

    private async Task CreateReport()
    {
        var personId = _personId!.Value;
        var report = _vm.ReportViewModel.GetReport(personId);
        Log.Information("Creating report {@Report}", report);
        await MortalityService.CreateReport(report);
        Log.Information("Created report");
        NavigationManager.NavigateTo(
            Constants.Routes.GetReportDetailsPageLink(HumanReadablePersonId, report.Id)
        );
    }

    private async Task UpdateReport()
    {
        var personId = _personId!.Value;
        var report = _vm.ReportViewModel.GetReport(personId);
        Log.Information("Updating report {@Report}", report);
        await MortalityService.UpdateReport(report);
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
