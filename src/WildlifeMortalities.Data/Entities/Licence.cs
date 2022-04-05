using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities
{
    public class Licence
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public LicenceType Type { get; set; }
        public List<Seal> Seals { get; set; } = new();
        public List<TrappedMortality> TrappedMortalities { get; set; } = new();
    }
}