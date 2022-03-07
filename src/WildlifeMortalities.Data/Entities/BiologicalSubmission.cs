namespace WildlifeMortalities.Data.Entities
{
    public class BiologicalSubmission
    {
        public int Id { get; set; }

        public int MortalityId { get; set; }
        public Mortality Mortality { get; set; }
    }
}