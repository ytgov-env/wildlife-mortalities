using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public abstract class Mortality
{
    public int Id { get; set; }
    public int MortalityReportId { get; set; }
    public MortalityReport MortalityReport { get; set; } = null!;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public Sex Sex { get; set; }
    public string Discriminator { get; set; } = null!;
}
