namespace WildlifeMortalities.Data.Entities
{
    public class GameManagementArea
    {
        public int Id { get; set; }
        public int Zone { get; set; }
        public int Subzone { get; set; }
        public int ZoneSubzone { get; }
        public List<Region> Regions { get; set; }
    }
}