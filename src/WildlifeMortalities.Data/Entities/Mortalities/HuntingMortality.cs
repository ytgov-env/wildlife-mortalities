using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities
{
    public class HuntingMortality : MortalityBase
    {
        public Sex Sex { get; set; }
        public int SealNumber { get; set; }
        public int GameManagementSubzone { get; set; }
        public int GameManagementUnit { get; set; }
        public string Landmark { get; set; }
    }
}
