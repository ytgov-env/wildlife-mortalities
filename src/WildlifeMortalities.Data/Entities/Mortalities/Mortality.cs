using NetTopologySuite.Geometries;
using WildlifeMortalities.Data.Entities.Reporters;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public abstract class Mortality
{
    public int Id { get; set; }
    public int ReporterId { get; set; }
    public Reporter Reporter { get; set; } = null!;
    public Point? Coordinates { get; set; }
    public string Discriminator { get; set; }
}
