using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities
{
    public class TrappingMortality : MortalityBase
    {
        public Sex Sex { get; set; }
        public int Quantity { get; set; }
        public string KillType { get; set; }
        public DateTime KillDate { get; set; }
        public int SealNumber { get; set; }
        public int RegisteredTrappingConcession { get; set; }
    }
}
