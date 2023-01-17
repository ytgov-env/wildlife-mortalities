using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class MortalityReportPage
{
    private readonly IList<IBrowserFile> _files = new List<IBrowserFile>();
    private EditContext _editContext;
    private int? _personId;
    private MortalityReportPageViewModel _vm;

    [Parameter]
    public string EnvClientId { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IMortalityService MortalityService { get; set; } = default!;

    [Inject]
    public ClientService ClientService { get; set; } = default!;

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

        switch (type)
        {
            case MortalityReportType.IndividualHunt:
                _vm.HuntedMortalityReportViewModel = new HuntedMortalityReportViewModel();
                _vm.OutfitterGuidedHuntReportViewModel = null;
                _vm.SpecialGuidedHuntReportViewModel = null;
                break;
            case MortalityReportType.OutfitterGuidedHunt:
                _vm.HuntedMortalityReportViewModel = null;
                _vm.OutfitterGuidedHuntReportViewModel = new OutfitterGuidedHuntReportViewModel();
                _vm.SpecialGuidedHuntReportViewModel = null!;
                break;
            case MortalityReportType.SpecialGuidedHunt:
                _vm.HuntedMortalityReportViewModel = null;
                _vm.OutfitterGuidedHuntReportViewModel = null;
                _vm.SpecialGuidedHuntReportViewModel = new SpecialGuidedHuntReportViewModel();
                break;
        }
    }

    private async Task CreateMortalityReport()
    {
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
                var validator = new HuntedMortalityReportViewModelValidator();
                var result = await validator.ValidateAsync(_vm.HuntedMortalityReportViewModel);
                if (result.IsValid)
                {
                    var report = _vm.HuntedMortalityReportViewModel!.GetReport(personId);
                    await MortalityService.CreateReport(report);
                }

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
                break;
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
