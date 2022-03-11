using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public class HuntedMortality : MortalityBase
    {
        public Sex Sex { get; set; }
        public int SealId { get; set; }
        public Seal Seal { get; set; }
        public GameManagementArea GameManagementArea { get; set; }
        public string Landmark { get; set; }
        public Point Location { get; set; }
    }
}
