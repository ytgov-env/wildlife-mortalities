using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities.Reports;

public class ReportPdf
{
    public int Id { get; set; }
    public int ReportId { get; set; }
    public Report Report { get; set; } = null!;

    //public int Version { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string FilePath { get; set; } = string.Empty;
}
