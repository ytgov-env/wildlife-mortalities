using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class MortalityReportPage
{
    private readonly Dictionary<int, bool> _validationMapper = new();

    private MortalityReportType _mortalityReportType;

    private AllSpecies _species;

    private MortalityViewModel? _mortalityViewModel;
    private MortalityReportViewModel? _mortalityReportViewModel;

    [Parameter]
    public int PersonId { get; set; }

    [Inject]
    private IDbContextFactory<AppDbContext> dbContextFactory { get; set; }

    public MortalityReportPage()
    {
        _validationMapper.Add(1, true); // type
        _validationMapper.Add(2, false); // species
        _validationMapper.Add(3, false); // report
        _validationMapper.Add(4, false); // mortality
    }

    private void SetStepValidation(int stepNumber, bool validationResult) =>
        _validationMapper[stepNumber] = validationResult;

    private void MortalityReportTypeChanged(MortalityReportType type)
    {
        _mortalityReportType = type;
    }

    private void SpeciesChanged(AllSpecies species)
    {
        _species = species;
    }

    private void SetMortalityViewModel(MortalityViewModel viewModel) =>
        _mortalityViewModel = viewModel;

    private void SetMortalityReportViewModel(MortalityReportViewModel viewModel) =>
        _mortalityReportViewModel = viewModel;

    private async Task CreateMortalityReport()
    {
        MortalityReport report = null;
        if (_mortalityReportType == MortalityReportType.Hunted)
        {
            report = new HuntedHarvestReport
            {
                Mortality = _mortalityViewModel.GetMortality(),
                Landmark = _mortalityReportViewModel.Landmark,
                Comments = _mortalityReportViewModel.Comments,
                ClientId = PersonId,
            };
        }
        else if (_mortalityReportType == MortalityReportType.Trapped)
        {
            report = new TrappedHarvestReport() { Comments = _mortalityReportViewModel.Comments, };
        }

        using var context = await dbContextFactory.CreateDbContextAsync();
        context.MortalityReports.Add(report);
        await context.SaveChangesAsync();
        //await Service.CreaeHavestReport(report);
    }
}
