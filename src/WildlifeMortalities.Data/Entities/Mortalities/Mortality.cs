using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public abstract class Mortality
{
    public int Id { get; set; }
    public int ReporterId { get; set; }
    public Reporter Reporter { get; set; } = null!;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public Sex Sex { get; set; }
    public string Discriminator { get; set; } = null!;
}
