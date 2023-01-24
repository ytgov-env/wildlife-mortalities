using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports;

public abstract class Report
{
    public int Id { get; set; }

    //Todo configure number generation
    public string Number { get; set; } = string.Empty;
    public DateTimeOffset DateSubmitted { get; set; }

    //public User CreatedBy { get; set; } = null!;
    //public User LastModifiedBy { get; set; } = null!;
    public ReportPdf? Pdf { get; set; }

    public abstract IEnumerable<Mortality> GetMortalities();
}
