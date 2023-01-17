using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports;

public abstract class Report
{
    public int Id { get; set; }
    public DateTimeOffset DateSubmitted { get; set; }

    public abstract IEnumerable<Mortality> GetMortalities();
}
