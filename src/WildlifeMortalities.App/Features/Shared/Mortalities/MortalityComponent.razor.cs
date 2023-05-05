using Microsoft.AspNetCore.Components;
using WildlifeMortalities.App.Features.Reports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class MortalityComponent
{
    [CascadingParameter(Name = Constants.CascadingValues.ReportType)]
    public ReportType ReportType { get; set; }

    [Parameter]
    public bool DisableSpeciesSelection { get; set; }

    [Parameter]
    [EditorRequired]
    public MortalityWithSpeciesSelectionViewModel ViewModel { get; set; } = null!;

    public MortalityViewModel GetViewModel() => ViewModel.MortalityViewModel;

    private void SpeciesChanged(Species? value)
    {
        if (!value.HasValue)
        {
            return;
        }

        if (ViewModel.Species == value)
        {
            return;
        }

        ViewModel.Species = value;
        ViewModel.MortalityViewModel = MortalityViewModel.Create(value.Value);
    }
}
