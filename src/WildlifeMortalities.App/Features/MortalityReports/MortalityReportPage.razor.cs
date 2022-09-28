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

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    private void ReportTypeChanged(MortalityReportType type)
    {
        var typeBefore = _vm.MortalityReportType;
        _vm.MortalityReportType = type;

        _vm.HuntedMortalityReportViewModels.Clear();

        if (
            typeBefore == MortalityReportType.IndividualHunt
            && (
                type == MortalityReportType.OutfitterGuidedHunt
                || type == MortalityReportType.SpecialGuidedHunt
            )
        )
        {
            //_vm.HuntedMortalityReportViewModels.Add(_vm.HuntedMortalityReportViewModel);
        }
    }

    private async Task CreateMortalityReport()
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        //MortalityReport report = null;
        if (_vm.MortalityReportType == MortalityReportType.Conflict)
        {
            var report = new HumanWildlifeConflictMortalityReport { };
            context.MortalityReports.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.IndividualHunt)
        {
            var report = _vm.HuntedMortalityReportViewModel!.GetReport(PersonId);

            context.MortalityReports.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.OutfitterGuidedHunt)
        {
            var report = new OutfitterGuidedHuntReport { };
        }
        else if (_vm.MortalityReportType == MortalityReportType.SpecialGuidedHunt)
        {
            var report = new SpecialGuidedHuntReport
            {
                HuntedMortalityReports = _vm.HuntedMortalityReportViewModels
                    .Select(x => x.GetReport(PersonId))
                    .ToList(),
            };
        }
        else if (_vm.MortalityReportType == MortalityReportType.Trapped) { }

        await context.SaveChangesAsync();
        //await Service.CreateHarvestReport(report);
    }

    private async Task Add()
    {
        var parameters = new DialogParameters
        {
            [nameof(AddHuntedMortalityReportDialog.ReportType)] = _vm.MortalityReportType
        };

        var dialog = DialogService.Show<AddHuntedMortalityReportDialog>("", parameters);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            _vm.HuntedMortalityReportViewModels.Add(result.Data as HuntedMortalityReportViewModel);
        }
    }

    private async Task Edit(HuntedMortalityReportViewModel viewModel)
    {
        var parameters = new DialogParameters
        {
            [nameof(EditHuntedMortalityReportDialog.ViewModel)] = viewModel
        };

        var dialog = DialogService.Show<EditHuntedMortalityReportDialog>("", parameters);
        var result = await dialog.Result;
    }

    private void Delete(HuntedMortalityReportViewModel viewModel)
    {
        _vm.HuntedMortalityReportViewModels.Remove(viewModel);
    }
}
