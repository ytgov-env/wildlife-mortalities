using NetTopologySuite.Geometries;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WoodBisonMortality : Mortality
{
    public Sex Sex { get; set; }
    public int? GameManagementAreaId { get; set; }
    public GameManagementArea? GameManagementArea { get; set; }
    public string? Landmark { get; set; }
}
