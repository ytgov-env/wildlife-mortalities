using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public interface ISingleMortalityReport
{
    public Mortality GetMortality();
    public Activity GetActivity();
}
