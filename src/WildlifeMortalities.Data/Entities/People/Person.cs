using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.People;

public abstract class Person
{
    public int Id { get; set; }
    public List<Mortality> Mortalities { get; set; } = new();
}
