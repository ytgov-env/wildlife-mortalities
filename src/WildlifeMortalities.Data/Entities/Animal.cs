using WildlifeMortalities.Data.Entities;

namespace WildlifeMortalities.Data
{
    public class Animal
    {
        public int Id { get; set; }
        public string Species { get; set; }
        public int MortalityId { get; set; }
        public Mortality Mortality { get; set; }
    }
}