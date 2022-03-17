namespace WildlifeMortalities.Data.Entities
{
    public class HarvestReport
    {
        public int Id { get; set; }
        public DateTime DateReported { get; set; }
        public int MortalityId { get; set; }
        public List<MortalityBase> Mortalities { get; set; }
    }
}