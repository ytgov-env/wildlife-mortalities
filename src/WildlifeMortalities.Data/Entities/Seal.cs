namespace WildlifeMortalities.Data.Entities
{
    public class Seal
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int LicenceId { get; set; }
        public Licence Licence { get; set; } = null!;
        public List<HuntedMortality> HuntedMortalities { get; set; } = new();
    }
}