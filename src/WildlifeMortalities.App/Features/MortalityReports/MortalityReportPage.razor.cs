using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.GuideReports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class MortalityReportPage
{
    private readonly IList<IBrowserFile> files = new List<IBrowserFile>();
    private EditContext _editContext;
    private MortalityReportPageViewModel _vm;

    [Parameter] public int PersonId { get; set; }

    [Inject] private IDbContextFactory<AppDbContext> dbContextFactory { get; set; }

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

        //if (
        //    typeBefore == MortalityReportType.IndividualHunt
        //    && (
        //        type == MortalityReportType.OutfitterGuidedHunt
        //        || type == MortalityReportType.SpecialGuidedHunt
        //    )
        //)
        //{
        //    _vm.HuntedMortalityReportViewModels.Add(_vm.HuntedMortalityReportViewModel);
        //}
    }

    private async Task CreateMortalityReport()
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        //MortalityReport report = null;
        if (_vm.MortalityReportType == MortalityReportType.Conflict)
        {
            var report = new HumanWildlifeConflictMortalityReport();
            context.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.IndividualHunt)
        {
            var report = _vm.HuntedMortalityReportViewModel!.GetReport(PersonId);

            context.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.OutfitterGuidedHunt)
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
        }
        else if (_vm.MortalityReportType == MortalityReportType.SpecialGuidedHunt)
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
        }
        else if (_vm.MortalityReportType == MortalityReportType.Trapped)
        {
        }

        await context.SaveChangesAsync();
        //await Service.CreateHarvestReport(report);
    }

    private void UploadFiles(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            files.Add(file);
        }
        //TODO upload the files to the server
    }
}
