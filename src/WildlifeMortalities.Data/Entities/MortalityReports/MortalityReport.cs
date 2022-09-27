using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities;

public abstract class MortalityReport
{
    public int Id { get; set; }
    public Mortality Mortality { get; set; } = null!;
}
