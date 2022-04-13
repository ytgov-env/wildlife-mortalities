namespace WildlifeMortalities.Data.Entities;

public class HarvestReportBase
{
    public int Id { get; set; }
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset DateCompleted { get; set; }
    public bool IsComplete { get; set; }
}
