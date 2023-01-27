using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public interface IMultipleMortalitiesReport
{
    IEnumerable<Mortality> GetMortalities();
}
