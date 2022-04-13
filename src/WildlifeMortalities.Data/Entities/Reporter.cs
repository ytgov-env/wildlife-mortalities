namespace WildlifeMortalities.Data.Entities;

public class Reporter
{
    public int Id { get; set; }
    public List<MortalityBase> Mortalities { get; set; } = new();
}
