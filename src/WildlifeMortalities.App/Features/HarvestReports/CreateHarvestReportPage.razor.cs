namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class CreateHarvestReportPage
{
    private int _selectedIndex;
    private readonly Dictionary<int, bool> _validationMapper = new();

    public CreateHarvestReportPage()
    {
        _validationMapper.Add(1, false);
        _validationMapper.Add(2, false);
        _validationMapper.Add(3, false);
    }

    private void OnNextClicked() => ++_selectedIndex;

    private void OnPreviousClicked() => --_selectedIndex;

    private void SetStepValidation(int stepNumber, bool validationResult) =>
        _validationMapper[stepNumber] = validationResult;
}
