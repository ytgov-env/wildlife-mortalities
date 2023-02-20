using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public interface IMultipleMortalitiesReport
{
    IEnumerable<Mortality> GetMortalities();
    IEnumerable<Activity> GetActivities();
}
