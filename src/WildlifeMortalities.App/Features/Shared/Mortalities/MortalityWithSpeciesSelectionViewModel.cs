using System.Text.Json.Serialization;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public class MortalityWithSpeciesSelectionViewModel
{
    public Species? Species { get; set; }

    [JsonConverter(typeof(MostConcreteClassJsonConverter<MortalityViewModel>))]
    public MortalityViewModel MortalityViewModel { get; set; }
}
