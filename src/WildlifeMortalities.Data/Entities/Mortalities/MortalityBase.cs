using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public abstract class MortalityBase
    {
        public int Id { get; set; }
        public AllSpecies Species { get; set; }
        public BiologicalSubmission BiologicalSubmission { get; set; }
        public HarvestReport HarvestReport { get; set; }
    }
}
