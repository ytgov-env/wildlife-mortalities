using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public partial class SelectSpeciesComponent
{
    private static readonly Dictionary<TrappedSpecies, AllSpecies> s_trappedMapper =
        new()
        {
            { TrappedSpecies.GreyWolf, AllSpecies.GreyWolf },
            { TrappedSpecies.Wolverine, AllSpecies.Wolverine }
        };

    private static readonly Dictionary<HuntedSpecies, AllSpecies> s_huntedMapper =
        new()
        {
            { HuntedSpecies.AmericanBlackBear, AllSpecies.AmericanBlackBear },
            { HuntedSpecies.Caribou, AllSpecies.Caribou },
            { HuntedSpecies.Coyote, AllSpecies.Coyote },
            { HuntedSpecies.Elk, AllSpecies.Elk },
            { HuntedSpecies.GreyWolf, AllSpecies.GreyWolf },
            { HuntedSpecies.GrizzlyBear, AllSpecies.GrizzlyBear },
            { HuntedSpecies.Moose, AllSpecies.Moose },
            { HuntedSpecies.MountainGoat, AllSpecies.MountainGoat },
            { HuntedSpecies.MuleDeer, AllSpecies.MuleDeer },
            { HuntedSpecies.ThinhornSheep, AllSpecies.ThinhornSheep },
            { HuntedSpecies.WhiteTailedDeer, AllSpecies.WhiteTailedDeer },
            { HuntedSpecies.Wolverine, AllSpecies.Wolverine },
            { HuntedSpecies.WoodBison, AllSpecies.WoodBison },
        };

    [Parameter]
    public MortalityReportType ReportType { get; set; }

    [Parameter]
    public EventCallback<AllSpecies> SpeciesChanged { get; set; }

    protected override void FieldsChanged()
    {
        if (ViewModel.HuntedSpecies.HasValue)
        {
            SpeciesChanged.InvokeAsync(s_huntedMapper[ViewModel.HuntedSpecies.Value]);
        }
        else if (ViewModel.TrappedSpecies.HasValue)
        {
            SpeciesChanged.InvokeAsync(s_trappedMapper[ViewModel.TrappedSpecies.Value]);
        }
    }
}
