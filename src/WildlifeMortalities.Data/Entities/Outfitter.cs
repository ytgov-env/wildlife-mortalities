namespace WildlifeMortalities.Data.Entities
{
    public class Outfitter
    {
        public int Id { get; set; }
        public int HarvestReportId { get; set; }
        public HarvestReport HarvestReport { get; set; }
    }
}