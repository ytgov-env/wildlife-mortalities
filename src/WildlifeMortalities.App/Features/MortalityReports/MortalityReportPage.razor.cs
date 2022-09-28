using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.GuideReports;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class MortalityReportPage
{
    private EditContext _editContext;
    private MortalityReportPageViewModel _vm;

    [Parameter]
    public int PersonId { get; set; }

    protected override void OnInitialized()
    {
        _vm = new MortalityReportPageViewModel();
        _editContext = new(_vm);
    }

    [Inject]
    private IDbContextFactory<AppDbContext> dbContextFactory { get; set; }

    private void ReportTypeChanged(MortalityReportType type)
    {
        var typeBefore = _vm.MortalityReportType;
        _vm.MortalityReportType = type;

        switch (type)
        {
            case MortalityReportType.IndividualHunt:
                _vm.HuntedMortalityReportViewModel = new();
                _vm.OutfitterGuidedHuntReportViewModel = null;
                _vm.SpecialGuidedHuntReportViewModel = null;
                break;
            case MortalityReportType.OutfitterGuidedHunt:
                _vm.HuntedMortalityReportViewModel = null;
                _vm.OutfitterGuidedHuntReportViewModel = new();
                _vm.SpecialGuidedHuntReportViewModel = null!;
                break;
            case MortalityReportType.SpecialGuidedHunt:
                _vm.HuntedMortalityReportViewModel = null;
                _vm.OutfitterGuidedHuntReportViewModel = null;
                _vm.SpecialGuidedHuntReportViewModel = new();
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
            var report = new HumanWildlifeConflictMortalityReport { };
            context.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.IndividualHunt)
        {
            var report = _vm.HuntedMortalityReportViewModel!.GetReport(PersonId);

            context.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.OutfitterGuidedHunt)
        {
            var report = new OutfitterGuidedHuntReport
            {
                HuntedMortalityReports =
                    _vm.OutfitterGuidedHuntReportViewModel.HuntedMortalityReportViewModels
                        .Select(x => x.GetReport(PersonId))
                        .ToList(),
            };
            context.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.SpecialGuidedHunt)
        {
            var report = new SpecialGuidedHuntReport
            {
                HuntedMortalityReports =
                    _vm.SpecialGuidedHuntReportViewModel.HuntedMortalityReportViewModels
                        .Select(x => x.GetReport(PersonId))
                        .ToList(),
            };
            context.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.Trapped) { }

        await context.SaveChangesAsync();
        //await Service.CreateHarvestReport(report);
    }
}
