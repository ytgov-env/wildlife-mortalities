using Microsoft.AspNetCore.Components;
using WildlifeMortalities.App.Features.Reports;
using WildlifeMortalities.App.Features.Shared.Mortalities.AmericanBlackBear;
using WildlifeMortalities.App.Features.Shared.Mortalities.Elk;
using WildlifeMortalities.App.Features.Shared.Mortalities.GrizzlyBear;
using WildlifeMortalities.App.Features.Shared.Mortalities.ThinhornSheep;
using WildlifeMortalities.App.Features.Shared.Mortalities.WoodBison;
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
        if (value.HasValue == false)
        {
            return;
        }

        if (ViewModel.Species == value)
        {
            return;
        }

        ViewModel.Species = value;

        var viewModel = MortalityViewModel.Create(value.Value);

        ViewModel.MortalityViewModel = viewModel;
    }
}
