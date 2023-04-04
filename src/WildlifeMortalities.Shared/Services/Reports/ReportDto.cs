using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Shared.Services.Reports;

public class ReportDto
{
    public int Id { get; set; }
    public string HumanReadableId { get; set; } = string.Empty;
    public string? EnvClientId { get; set; }
    public string? BadgeNumber { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? OutfitterGuidedHuntResult { get; set; }
    public string? SpecialGuidedHuntResult { get; set; }
    public Season Season { get; set; } = null!;
    public IEnumerable<Species> SpeciesCollection { get; set; } = Enumerable.Empty<Species>();
    public DateTimeOffset DateSubmitted { get; set; }
}
