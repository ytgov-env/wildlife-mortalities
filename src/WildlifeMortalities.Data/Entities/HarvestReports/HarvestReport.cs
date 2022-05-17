using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class HarvestReport
{
    public int Id { get; set; }
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset DateCompleted { get; set; }
    public HarvestReportStatus Status { get; set; }
}
