namespace WildlifeMortalities.Data.Entities
{
    public class GameManagementUnit
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public List<GameManagementAreaSpecies> GameManagementAreaSpecies { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
    }
}