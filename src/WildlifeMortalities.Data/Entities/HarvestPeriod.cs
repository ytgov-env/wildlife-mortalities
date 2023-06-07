namespace WildlifeMortalities.Data.Entities;

public enum SexRestriction
{
    OnlyMale,
    OnlyFemale,
    Both
}

public class SpeciesSexRestrictionRelation
{
    public Species Species { get; set; }
    public SexRestriction SexRestriction { get; set; }
}

public class HarvestPeriod
{
    public int Id { get; set; }
    public List<GameManagementArea> GameManagementAreas { get; set; } = null!;
    public List<SpeciesSexRestrictionRelation> SpeciesSexRestrictionRelations { get; set; } = null!;
    public Season Season { get; set; } = null!;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}
