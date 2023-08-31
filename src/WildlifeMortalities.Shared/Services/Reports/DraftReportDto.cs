using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.App.Features.Reports;

public class DraftReportDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateLastModified { get; set; }
}
