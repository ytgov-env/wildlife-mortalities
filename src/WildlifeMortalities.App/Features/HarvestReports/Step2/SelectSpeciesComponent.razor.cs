using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class SelectSpeciesComponent
{
    [Parameter]
    public HarvestReportType ReportType { get; set; }

    private static readonly Dictionary<TrappedSpecies, AllSpecies> _trappedMapper = new()
    {
        { TrappedSpecies.GreyWolf, AllSpecies.GreyWolf },
        { TrappedSpecies.Wolverine, AllSpecies.Wolverine }
    };

    private static readonly Dictionary<HuntedSpecies, AllSpecies> _huntedMapper = new()
    {
        { HuntedSpecies.AmericanBlackBear, AllSpecies.AmericanBlackBear },
        { HuntedSpecies.BarrenGroundCaribou, AllSpecies.BarrenGroundCaribou },
        { HuntedSpecies.Coyote, AllSpecies.Coyote },
        { HuntedSpecies.Elk, AllSpecies.Elk },
        { HuntedSpecies.GreyWolf, AllSpecies.GreyWolf },
        { HuntedSpecies.GrizzlyBear, AllSpecies.GrizzlyBear },
        { HuntedSpecies.Moose, AllSpecies.Moose },
        { HuntedSpecies.MountainGoat, AllSpecies.MountainGoat },
        { HuntedSpecies.MuleDeer, AllSpecies.MuleDeer },
        { HuntedSpecies.ThinhornSheep, AllSpecies.ThinhornSheep },
        { HuntedSpecies.Wolverine, AllSpecies.Wolverine },
        { HuntedSpecies.WoodBison, AllSpecies.WoodBison },
        { HuntedSpecies.WoodlandCaribou, AllSpecies.WoodlandCaribou }
    };

    protected override void FieldsChanged()
    {
        if (_viewModel.HuntedSpecies.HasValue)
        {
            SpeciesChanged.InvokeAsync(_huntedMapper[_viewModel.HuntedSpecies.Value]);
        }
        else if (_viewModel.TrappedSpecies.HasValue)
        {
            SpeciesChanged.InvokeAsync(_trappedMapper[_viewModel.TrappedSpecies.Value]);
        }
    }

    [Parameter]
    public EventCallback<AllSpecies> SpeciesChanged { get; set; }
}
