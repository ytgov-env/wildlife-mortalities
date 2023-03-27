namespace WildlifeMortalities.Shared.Services.Reports;

public class ReportDto
{
    public int Id { get; set; }
    public string HumanReadableId { get; set; } = string.Empty;
    public string? EnvClientId { get; set; }
    public string? BadgeNumber { get; set; }

    //int PersonId,
    public string Type { get; set; } = string.Empty;
    public string? OutfitterGuidedHuntResult { get; set; }
    public string? SpecialGuidedHuntResult { get; set; }
    public string Season { get; set; } = string.Empty;
    public IEnumerable<string> Species { get; set; } = Enumerable.Empty<string>();
    public DateTimeOffset DateSubmitted { get; set; }
}
