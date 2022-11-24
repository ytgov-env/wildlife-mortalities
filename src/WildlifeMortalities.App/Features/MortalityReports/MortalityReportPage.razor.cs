using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.GuidedReports;
using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class MortalityReportPage
{
    private readonly IList<IBrowserFile> _files = new List<IBrowserFile>();
    private EditContext _editContext;
    private MortalityReportPageViewModel _vm;

    [Parameter] public int PersonId { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Inject] private IDbContextFactory<AppDbContext> DbContextFactory { get; set; }

    protected override void OnInitialized()
    {
        _vm = new MortalityReportPageViewModel();
        _editContext = new EditContext(_vm);
    }

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
        await using var context = await DbContextFactory.CreateDbContextAsync();

        switch (_vm.MortalityReportType)
        {
            //MortalityReport report = null;
            case MortalityReportType.Conflict:
                {
                    var report = new HumanWildlifeConflictMortalityReport();
                    context.Add(report);
                    break;
                }
            case MortalityReportType.IndividualHunt:
                {
                    var validator = new HuntedMortalityReportViewModelValidator();
                    var result = await validator.ValidateAsync(_vm.HuntedMortalityReportViewModel);
                    if (result.IsValid)
                    {
                        var report = _vm.HuntedMortalityReportViewModel!.GetReport(PersonId);
                        context.Add(report);
                    }

                    break;
                }
            case MortalityReportType.OutfitterGuidedHunt:
                {
                    // Clear mortality reports if the hunter wasn't successful
                    var outfitterViewModel = _vm.OutfitterGuidedHuntReportViewModel;
                    if (outfitterViewModel!.Result is not GuidedHuntResult.SuccessfulHunt)
                    {
                        outfitterViewModel.HuntedMortalityReportViewModels.Clear();
                    }

                    var report = new OutfitterGuidedHuntReport
                    {
                        HuntedMortalityReports = outfitterViewModel.HuntedMortalityReportViewModels
                            .Select(x => x.GetReport(PersonId))
                            .ToList()
                    };
                    context.Add(report);
                    break;
                }
            case MortalityReportType.SpecialGuidedHunt:
                {
                    // Clear mortality reports if the hunter wasn't successful
                    var specialViewModel = _vm.SpecialGuidedHuntReportViewModel;
                    if (specialViewModel!.Result is not GuidedHuntResult.SuccessfulHunt)
                    {
                        specialViewModel.HuntedMortalityReportViewModels.Clear();
                    }

                    var report = new SpecialGuidedHuntReport
                    {
                        HuntedMortalityReports = specialViewModel.HuntedMortalityReportViewModels
                            .Select(x => x.GetReport(PersonId))
                            .ToList()
                    };
                    context.Add(report);
                    break;
                }
            case MortalityReportType.Trapped:
                break;
        }

        await context.SaveChangesAsync();
        //await Service.CreateMortalityReport(report);

        //Todo fix route parameter to use envClientId
        NavigationManager.NavigateTo($"reporters/clients/{PersonId}");
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
