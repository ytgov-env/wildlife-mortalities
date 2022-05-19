using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;
public class BlackBearBioSubmission
{
    public int MortalityId { get; set; }
    public BlackBearMortality Mortality { get; set; }
}
