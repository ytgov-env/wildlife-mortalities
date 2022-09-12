using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class CreateHarvestReportPage
{
    private int _selectedIndex;
    private readonly Dictionary<int, bool> _validationMapper = new();

    private HarvestReportType _harvestReportType;

    private AllSpecies _species;

    public CreateHarvestReportPage()
    {
        _validationMapper.Add(1, false);
        _validationMapper.Add(2, false);
        _validationMapper.Add(3, false);
    }

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
}
