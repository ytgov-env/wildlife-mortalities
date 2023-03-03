using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public partial class MortalityReportPage
{
    private readonly IList<IBrowserFile> _files = new List<IBrowserFile>();
    private EditContext _editContext;
    private bool _invalidSubmitDetected;
    private int? _personId;
    private SignaturePadComponent _signaturePad = null!;
    private MortalityReportPageViewModel _vm;

    [Parameter]
    public string EnvClientId { get; set; }

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
            _editContext.OnFieldChanged -= _editContext_OnFieldChanged;
        }

        _editContext = new EditContext(_vm);
        _editContext.OnFieldChanged += _editContext_OnFieldChanged;
    }

    protected override void OnInitialized()
    {
        _vm = new MortalityReportPageViewModel();
        CreateNewEditContext();
    }

    private void _editContext_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        if (_invalidSubmitDetected == false)
        {
            return;
        }

        _editContext.Validate();
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync() =>
        _personId = await ClientService.GetPersonIdByEnvClientId(EnvClientId);

    private void ReportTypeChanged(MortalityReportType type)
    {
        CreateNewEditContext();

        _vm.MortalityReportType = type;

        _vm.IndividualHuntedMortalityReportViewModel = null;
        _vm.OutfitterGuidedHuntReportViewModel = null;
        _vm.SpecialGuidedHuntReportViewModel = null;
        _vm.TrappedReportViewModel = null;

        switch (type)
        {
            case MortalityReportType.IndividualHunt:
                _vm.IndividualHuntedMortalityReportViewModel =
                    new IndividualHuntedMortalityReportViewModel();
                break;
            case MortalityReportType.OutfitterGuidedHunt:
                _vm.OutfitterGuidedHuntReportViewModel = new OutfitterGuidedHuntReportViewModel();
                break;
            case MortalityReportType.SpecialGuidedHunt:
                _vm.SpecialGuidedHuntReportViewModel = new SpecialGuidedHuntReportViewModel();
                break;
            case MortalityReportType.Trapped:
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

            await MortalityService.CreateDraftReport(content, personId);
            NavigationManager.NavigateTo($"reporters/clients/{EnvClientId}");
        }
    }

    private async Task CreateReport()
    {
        _ = await _signaturePad.GetSignature();

        var personId = _personId!.Value;

        switch (_vm.MortalityReportType)
        {
            case MortalityReportType.Conflict:
            {
                var report = new HumanWildlifeConflictMortalityReport();
                await MortalityService.CreateReport(report);
                NavigationManager.NavigateTo($"mortality-reports/{report.Id}");
                break;
            }
            case MortalityReportType.IndividualHunt:
            {
                var report = _vm.IndividualHuntedMortalityReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                NavigationManager.NavigateTo($"mortality-reports/{report.Id}");
                break;
            }
            case MortalityReportType.OutfitterGuidedHunt:
            {
                var report = _vm.OutfitterGuidedHuntReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                NavigationManager.NavigateTo($"mortality-reports/{report.Id}");
                break;
            }
            case MortalityReportType.SpecialGuidedHunt:
            {
                var report = _vm.SpecialGuidedHuntReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                NavigationManager.NavigateTo($"mortality-reports/{report.Id}");
                break;
            }
            case MortalityReportType.Trapped:
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
