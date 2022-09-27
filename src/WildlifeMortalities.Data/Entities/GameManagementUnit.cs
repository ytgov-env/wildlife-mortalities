namespace WildlifeMortalities.Data.Entities;

public class GameManagementUnit
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<GameManagementAreaSpecies> GameManagementAreaSpecies { get; set; } = null!;
    public DateTime ActiveFrom { get; set; }
    public DateTime ActiveTo { get; set; }
}
