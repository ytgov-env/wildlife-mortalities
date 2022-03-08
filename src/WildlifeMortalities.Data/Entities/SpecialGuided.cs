namespace WildlifeMortalities.Data.Entities
{
    public class SpecialGuided
    {
        public int Id { get; set; }
        public int HarvestReportId { get; set; }
        public HarvestReport HarvestReport { get; set; }
    }
}