using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public abstract class MortalityReport
{
    public int Id { get; set; }
    public Mortality Mortality { get; set; } = null!;
}
