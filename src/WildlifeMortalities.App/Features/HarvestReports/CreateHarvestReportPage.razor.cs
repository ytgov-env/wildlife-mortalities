using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Models;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class CreateHarvestReportPage
{
    private Int32 _selectedIndex = 0;
    private Dictionary<Int32, Boolean> _validationMapper = new Dictionary<int, bool>();

    public CreateHarvestReportPage()
    {
        _validationMapper.Add(1, false);
        _validationMapper.Add(21, false);
    }

    private void OnNextClicked() => _selectedIndex += 1;

    private void SetStepValidation(Int32 stepNunber, Boolean validationResult) =>
        _validationMapper[stepNunber] = validationResult;
}
