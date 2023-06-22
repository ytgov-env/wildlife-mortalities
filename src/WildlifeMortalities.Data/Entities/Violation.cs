using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities;

public class Violation
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RuleType Rule { get; set; }
    public ViolationSeverity Severity { get; set; }
    public Activity Activity { get; set; } = null!;

    public enum RuleType
    {
        BagLimit = 10,
        HarvestPeriod = 20,
        Authorization = 30,
    }

    public enum ViolationSeverity
    {
        InternalError = 10,
        PotentiallyIllegal = 20,
        Illegal = 30
    }
}
