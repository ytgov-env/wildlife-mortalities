using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reporters;

public abstract class Reporter
{
    public int Id { get; set; }
    public List<Mortality> Mortalities { get; set; } = new();
}
