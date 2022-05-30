using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities.Mortalities;
public class GreyWolfMortality : Mortality
{
    public int? TrappedHarvestReportId { get; set; }
    public TrappedHarvestReport? TrappedHarvestReport { get; set; }
}
