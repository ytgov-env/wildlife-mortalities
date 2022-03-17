namespace WildlifeMortalities.App.Data
{
    public class Seal
    {
        public int Id { get; set; }
        public string Species { get; set; } = "";
        public int Year { get; set; }
        public SealType SealType { get; set; }
        public string HarvestReportStatus { get; set; }
    }
}