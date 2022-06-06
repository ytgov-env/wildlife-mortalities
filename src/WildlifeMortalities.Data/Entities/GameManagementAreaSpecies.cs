using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class GameManagementAreaSpecies
{
    public int Id { get; set; }
    public HuntedSpeciesWithGameManagementArea Species { get; set; }
    public int GameManagementAreaId { get; set; }
    public GameManagementArea GameManagementArea { get; set; } = null!;
    public List<GameManagementAreaSchedule> Schedules { get; set; } = new();
    public List<GameManagementUnit?> GameManagementUnits { get; set; } = new();
}
