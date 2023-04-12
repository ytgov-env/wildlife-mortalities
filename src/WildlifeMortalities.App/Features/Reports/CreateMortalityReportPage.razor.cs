using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public partial class CreateMortalityReportPage : DbContextAwareComponent
{
    private readonly IList<IBrowserFile> _files = new List<IBrowserFile>();
    private EditContext _editContext;
    private bool _invalidSubmitDetected;
    private int? _personId;
    private SignaturePadComponent _signaturePad = null!;
    private CreateMortalityReportPageViewModel _vm;

    [Parameter]
    public int? DraftId { get; set; }

    [Parameter]
    public string EnvClientId { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IMortalityService MortalityService { get; set; } = default!;

    [Inject]
    public ClientService ClientService { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; }

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
        if (DraftId == null)
        {
            _vm = new CreateMortalityReportPageViewModel();
            CreateNewEditContext();
        }

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        _personId = await ClientService.GetPersonIdByEnvClientId(EnvClientId);

        if (DraftId != null)
        {
            var draft = await Context.DraftReports.FirstOrDefaultAsync(x => x.Id == DraftId.Value);
            if (draft == null)
            {
                return;
            }

            _vm = JsonSerializer.Deserialize<CreateMortalityReportPageViewModel>(
                draft.SerializedData
            )!;
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

        _vm.IndividualHuntedMortalityReportViewModel = null;
        _vm.OutfitterGuidedHuntReportViewModel = null;
        _vm.SpecialGuidedHuntReportViewModel = null;
        _vm.TrappedReportViewModel = null;

        switch (type)
        {
            case ReportType.IndividualHuntedMortalityReport:
                _vm.IndividualHuntedMortalityReportViewModel =
                    new IndividualHuntedMortalityReportViewModel();
                break;
            case ReportType.OutfitterGuidedHuntReport:
                _vm.OutfitterGuidedHuntReportViewModel = new OutfitterGuidedHuntReportViewModel();
                break;
            case ReportType.SpecialGuidedHuntReport:
                _vm.SpecialGuidedHuntReportViewModel = new SpecialGuidedHuntReportViewModel();
                break;
            case ReportType.TrappedMortalitiesReport:
                _vm.TrappedReportViewModel = new TrappedReportViewModel();
                break;
        }
    }

    private async Task CreateDraftReport()
    {
        if (_editContext.GetValidationMessages().Any())
        {
            var personId = _personId!.Value;
            var content = JsonSerializer.Serialize(_vm);

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
            NavigationManager.NavigateTo($"reporters/clients/{EnvClientId}");
        }
    }

    private async Task CreateReport()
    {
        //_ = await _signaturePad.GetSignature();

        var personId = _personId!.Value;

        switch (_vm.ReportType)
        {
            case ReportType.HumanWildlifeConflictMortalityReport:
            {
                var report = new HumanWildlifeConflictMortalityReport();
                await MortalityService.CreateReport(report);
                NavigationManager.NavigateTo($"mortality-reports/{report.Id}");
                break;
            }
            case ReportType.IndividualHuntedMortalityReport:
            {
                var report = _vm.IndividualHuntedMortalityReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                NavigationManager.NavigateTo($"mortality-reports/{report.Id}");
                break;
            }
            case ReportType.OutfitterGuidedHuntReport:
            {
                var report = _vm.OutfitterGuidedHuntReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                NavigationManager.NavigateTo($"mortality-reports/{report.Id}");
                break;
            }
            case ReportType.SpecialGuidedHuntReport:
            {
                var report = _vm.SpecialGuidedHuntReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                NavigationManager.NavigateTo($"mortality-reports/{report.Id}");
                break;
            }
            case ReportType.TrappedMortalitiesReport:
            {
                var report = _vm.TrappedReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                NavigationManager.NavigateTo($"mortality-reports/{report.Id}");
                break;
            }
        }

        //NavigationManager.NavigateTo($"reporters/clients/{EnvClientId}");
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
