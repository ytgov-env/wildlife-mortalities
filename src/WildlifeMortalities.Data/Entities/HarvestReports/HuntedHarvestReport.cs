using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities
{
    public class HuntedHarvestReport : HarvestReportBase
    {
        public int MortalityId { get; set; }
        public HuntedMortality Mortality { get; set; } = null!;
    }
}
