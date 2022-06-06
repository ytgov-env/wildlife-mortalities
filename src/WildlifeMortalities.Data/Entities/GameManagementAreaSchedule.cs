using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class GameManagementAreaSchedule
{
    public int Id { get; set; }
    public int GameManagementAreaSpeciesId { get; set; }
    public GameManagementAreaSpecies GameManagementAreaSpecies { get; set; } = null!;
    public GameManagementAreaStatus Status { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}
