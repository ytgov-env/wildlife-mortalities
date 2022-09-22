using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class MortalityReportPage
{
    private EditContext _editContext;
    private MortalityReportViewModel _vm;

    private MortalityViewModel? _mortalityViewModel;

    [Parameter]
    public int PersonId { get; set; }

    protected override void OnInitialized()
    {
        _vm = new MortalityReportViewModel();
        _editContext = new(_vm);
    }

    [Inject]
    private IDbContextFactory<AppDbContext> dbContextFactory { get; set; }


    private void SetMortalityViewModel(MortalityViewModel viewModel) =>
        _mortalityViewModel = viewModel;

    private async Task CreateMortalityReport()
    {
        MortalityReport report = null;
        if (_vm.MortalityReportType == MortalityReportType.Hunted)
        {
            report = new HuntedHarvestReport
            {
                Mortality = _mortalityViewModel.GetMortality(),
                Landmark = _vm.Landmark,
                Comments = _vm.Comments,
                ClientId = PersonId,
            };
        }
        else if (_vm.MortalityReportType == MortalityReportType.Trapped)
        {
            report = new TrappedHarvestReport() { Comments = _vm.Comments, };
        }

        using var context = await dbContextFactory.CreateDbContextAsync();
        context.MortalityReports.Add(report);
        await context.SaveChangesAsync();
        //await Service.CreaeHavestReport(report);
    }
}
