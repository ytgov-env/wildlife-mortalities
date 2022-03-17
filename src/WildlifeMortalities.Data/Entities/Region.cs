namespace WildlifeMortalities.Data.Entities
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GameManagementArea> GameManagementAreas { get; set; }
    }
}