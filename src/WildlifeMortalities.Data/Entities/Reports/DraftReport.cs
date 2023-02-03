using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Reports;

public class DraftReport
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; }
    public DateTimeOffset LastModifiedDate { get; set; }
    public string SerializedData { get; set; }
}
