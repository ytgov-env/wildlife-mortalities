using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class Violation
{
    public int Id { get; set; }
    public ViolationType Type { get; set; }
    public string Message { get; set; } = "";
    public List<MortalityReport> HarvestReports { get; set; } = null!;
}
