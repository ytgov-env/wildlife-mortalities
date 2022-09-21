using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class HuntedHarvestReport : MortalityReport
{

    public int ClientId { get; set; }
    public Client Client { get; set; }
    public int GmaSpeciesId { get; set; }
    public GameManagementAreaSpecies GmaSpecies { get; set; } = null!;
    public string Landmark { get; set; } = string.Empty;
    public string? TemporarySealNumber { get; set; }
    public int? SealId { get; set; }
    public Seal? Seal { get; set; }
    public HarvestReportStatus Status { get; set; }
    public string Comments { get; set; } = "";
    public List<Violation> Violations { get; set; } = null!;
}
