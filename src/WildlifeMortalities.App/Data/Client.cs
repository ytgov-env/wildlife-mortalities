namespace WildlifeMortalities.App.Data
{
    public class Client
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public bool HasActiveHuntingLicence { get; set; }
        public bool HasActiveTrappingLicence { get; set; }
        public int ActiveSeals { get; set; }
        public List<Seal> Seals { get; set; } = new();
    }
}