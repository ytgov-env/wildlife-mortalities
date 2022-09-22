using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.GuideReports;
public class SpecialGuideReport
{
    public int Id { get; set; }
    public int GuideId { get; set; }
    public Client Guide { get; set; } = null!;
    public List<HuntedHarvestReport> HuntedHarvestReports { get; set; } = null!;
}
