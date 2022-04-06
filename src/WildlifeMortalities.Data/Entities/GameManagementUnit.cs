namespace WildlifeMortalities.Data.Entities;

public class GameManagementUnit
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<GameManagementAreaSpecies> GameManagementAreaSpecies { get; set; } = new();
    public DateTime ActiveFrom { get; set; }
    public DateTime ActiveTo { get; set; }
}
