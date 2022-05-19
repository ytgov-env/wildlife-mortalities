using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;
public class BisonBioSubmission : BioSubmission
{
    public int MortalityId { get; set; }
    public BisonMortality Mortality { get; set; }
}
