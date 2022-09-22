using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.GuideReports;

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
        using var context = await dbContextFactory.CreateDbContextAsync();

        //MortalityReport report = null;
        if (_vm.MortalityReportType == MortalityReportType.Conflict)
        {
            var report = new HumanWildlifeConflictReport { };
            context.MortalityReports.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.Hunted)
        {
            var report = new HuntedHarvestReport
            {
                Mortality = _mortalityViewModel.GetMortality(),
                Landmark = _vm.Landmark,
                Comments = _vm.Comments,
                ClientId = PersonId,
            };
            context.MortalityReports.Add(report);
        }
        else if (_vm.MortalityReportType == MortalityReportType.Outfitted)
        {
            var report = new OutfitterGuideReport { };
        }
        else if (_vm.MortalityReportType == MortalityReportType.SpecialGuided)
        {
            var report = new SpecialGuideReport { };
        }
        else if (_vm.MortalityReportType == MortalityReportType.Trapped) { }

        await context.SaveChangesAsync();
        //await Service.CreateHarvestReport(report);
    }
}
