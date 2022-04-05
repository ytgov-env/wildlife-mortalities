using NetTopologySuite.Geometries;
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
        public HuntedHarvestReport HuntedHarvestReport { get; set; }
    }
}