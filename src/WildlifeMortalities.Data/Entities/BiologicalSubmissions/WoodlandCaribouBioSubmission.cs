using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;
public class WoodlandCaribouBioSubmission : BioSubmission
{
    public int MortalityId { get; set; }
    public WoodlandCaribouMortality Mortality { get; set; }
}
