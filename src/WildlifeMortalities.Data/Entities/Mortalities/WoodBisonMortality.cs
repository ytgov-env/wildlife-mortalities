namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WoodBisonMortality : Mortality
{
    public int? GameManagementAreaId { get; set; }
    public GameManagementArea? GameManagementArea { get; set; }
    public string? Landmark { get; set; }
}
