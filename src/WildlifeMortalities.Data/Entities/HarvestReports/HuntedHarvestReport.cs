using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities;

public class HuntedHarvestReport : HarvestReport
{
    public int MortalityId { get; set; }
    public Mortality Mortality { get; set; } = null!;
    public int GmaSpeciesId { get; set; }
    public GameManagementAreaSpecies GmaSpecies { get; set; } = null!;
    public string Landmark { get; set; } = string.Empty;
    public string? TemporarySealNumber { get; set; }
    public int? SealId { get; set; }
    public Seal? Seal { get; set; }
}
