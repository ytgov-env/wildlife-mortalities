using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Reporters;

public class DraftReportDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public DateTimeOffset DateLastModified { get; set; }
    public DateTimeOffset DateSubmitted { get; set; }
}
