using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public partial class MortalityReportPage
{
    private readonly IList<IBrowserFile> _files = new List<IBrowserFile>();
    private EditContext _editContext;
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

    protected override void OnInitialized()
    {
        _vm = new MortalityReportPageViewModel();
        _editContext = new EditContext(_vm);
    }

    protected override async Task OnInitializedAsync() =>
        _personId = await ClientService.GetPersonIdByEnvClientId(EnvClientId);

    private void ReportTypeChanged(MortalityReportType type)
    {
        var typeBefore = _vm.MortalityReportType;
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
        var personId = _personId!.Value;
        var content = JsonSerializer.Serialize(_vm);

        await MortalityService.CreateDraftReport(content, personId);
    }

    private async Task CreateReport()
    {
        var signature = await _signaturePad.GetSignature();

        var personId = _personId!.Value;

        switch (_vm.MortalityReportType)
        {
            case MortalityReportType.Conflict:
            {
                var report = new HumanWildlifeConflictMortalityReport();
                await MortalityService.CreateReport(report);
                break;
            }
            case MortalityReportType.IndividualHunt:
            {
                var report = _vm.IndividualHuntedMortalityReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                break;
            }
            case MortalityReportType.OutfitterGuidedHunt:
            {
                var report = _vm.OutfitterGuidedHuntReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                break;
            }
            case MortalityReportType.SpecialGuidedHunt:
            {
                var report = _vm.SpecialGuidedHuntReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                break;
            }
            case MortalityReportType.Trapped:
            {
                var report = _vm.TrappedReportViewModel!.GetReport(personId);
                await MortalityService.CreateReport(report);
                break;
            }
        }

        NavigationManager.NavigateTo($"reporters/clients/{EnvClientId}");
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
