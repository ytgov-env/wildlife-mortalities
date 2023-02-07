using WildlifeMortalities.App.Features.Shared.Mortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public class MortalityWithSpeciesSelectionViewModel
{
    public Species? Species { get; set; }
    public MortalityViewModel MortalityViewModel { get; set; }
}
