using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities
{
    public class HarvestReport
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime DateReported { get; set; }
        public int MortalityId { get; set; }
        public MortalityBase Mortality { get; set; }
        public Outfitter? Outfitter { get; set; }
        public SpecialGuided? SpecialGuided { get; set; }
    }
}
