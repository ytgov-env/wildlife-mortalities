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
    private MortalityReportViewModel _vm;

    [Parameter]
    public int PersonId { get; set; }

    protected override void OnInitialized()
    {
        _vm = new MortalityReportViewModel();
        _editContext = new(_vm);
    }

    [Inject]
    private IDbContextFactory<AppDbContext> dbContextFactory { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    private void SetMortalityViewModel(MortalityViewModel viewModel) =>
        _vm.MortalityViewModel = viewModel;

    private async Task CreateMortalityReport()
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        //MortalityReport report = null;
        if (_vm.MortalityReportType == MortalityReportType.Conflict)
        {
            var report = new HumanWildlifeConflictReport { };
            context.MortalityReports.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.IndividualHunt)
        {
            var report = new IndividualHuntReport
            {
                Mortality = _vm.MortalityViewModel.GetMortality(),
                Landmark = _vm.Landmark,
                Comment = _vm.Comment,
                ClientId = PersonId,
            };
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
                IndividualHuntReports = _vm.MortalityViewModels
                    .Select(
                        x =>
                            new IndividualHuntReport
                            {
                                Comment = _vm.Comment,
                                Mortality = x.GetMortality(),
                            }
                    )
                    .ToList(),
            };
        }
        else if (_vm.MortalityReportType == MortalityReportType.Trapped) { }

        await context.SaveChangesAsync();
        //await Service.CreateHarvestReport(report);
    }

    private async Task AddMortality()
    {
        var parameters = new DialogParameters
        {
            [nameof(AddMortalityDialog.ReportType)] = _vm.MortalityReportType
        };

        var dialog = DialogService.Show<AddMortalityDialog>("", parameters);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            _vm.MortalityViewModels.Add(result.Data as MortalityViewModel);
        }
    }

    private async Task Edit(MortalityViewModel viewModel)
    {
        var parameters = new DialogParameters
        {
            [nameof(EditMortalityDialog.ViewModel)] = viewModel
        };

        var dialog = DialogService.Show<EditMortalityDialog>("", parameters);
        var result = await dialog.Result;
    }

    private void Delete(MortalityViewModel viewModel)
    {
        _vm.MortalityViewModels.Remove(viewModel);
    }
}
