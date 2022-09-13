using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class HarvestReportPage
{
    private int _selectedIndex;
    private readonly Dictionary<int, bool> _validationMapper = new();

    private HarvestReportType _harvestReportType;

    private AllSpecies _species;

    private MortalityViewModel? _mortalityViewModel;
    private HarvestReportViewModel? _harvestReportViewModel;

    [Parameter]
    public int ReporterId { get; set; }

    [Inject]
    private IDbContextFactory<AppDbContext> dbContextFactory { get; set; }

    public HarvestReportPage()
    {
        _validationMapper.Add(1, true); //harvest type
        _validationMapper.Add(2, false); // species
        _validationMapper.Add(3, false); // report
        _validationMapper.Add(4, false); // mortality
    }

    protected override async Task OnInitializedAsync() { }

    private void SetStepValidation(int stepNumber, bool validationResult) =>
        _validationMapper[stepNumber] = validationResult;

    private void HarvestReportTypeChanged(HarvestReportType type)
    {
        _harvestReportType = type;
    }

    private void SpeciesChanged(AllSpecies species)
    {
        _species = species;
    }

    private void SetMortalityViewModel(MortalityViewModel viewModel) =>
        _mortalityViewModel = viewModel;

    private void SetHarvestReportViewModel(HarvestReportViewModel viewModel) =>
        _harvestReportViewModel = viewModel;

    private async Task CreateHarvestReport()
    {
        HarvestReport report = null;
        if (_harvestReportType == HarvestReportType.Hunting)
        {
            report = new HuntedHarvestReport
            {
                Mortality = _mortalityViewModel.GetMortality(ReporterId),
                Landmark = _harvestReportViewModel.Landmark,
                Comments = _harvestReportViewModel.Comments,
            };
        }
        else
        {
            report = new TrappedHarvestReport() { Comments = _harvestReportViewModel.Comments, };
        }

        using var context = await dbContextFactory.CreateDbContextAsync();
        context.HarvestReports.Add(report);
        await context.SaveChangesAsync();
        //await Service.CreaeHavestReport(report);
    }
}
