namespace WildlifeMortalities.App.Data
{
    public class HarvestReport
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ClientId { get; set; }
        public string Species { get; set; }
        public string Status { get; set; }
        public DateTime KillDate { get; set; }
        public string Subzone { get; set; } = "";
        public string Sex { get; set; } = "";
    }
}