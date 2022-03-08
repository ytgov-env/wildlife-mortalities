using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities
{
    public abstract class MortalityBase
    {
        public int Id { get; set; }
        public Species Species { get; set; }
        public BiologicalSubmission BiologicalSubmission { get; set; }
        public HarvestReport HarvestReport { get; set; }
    }
}
