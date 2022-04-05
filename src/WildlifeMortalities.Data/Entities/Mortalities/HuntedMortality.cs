using NetTopologySuite.Geometries;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public class HuntedMortality : MortalityBase
    {
        public Sex Sex { get; set; }
        public int SealId { get; set; }
        public Seal Seal { get; set; } = null!;
        public GameManagementArea GameManagementArea { get; set; } = null!;
        public string Landmark { get; set; } = string.Empty;
        public Point Location { get; set; } = null!;
        public HuntedHarvestReport HuntedHarvestReport { get; set; } = null!;
    }
}