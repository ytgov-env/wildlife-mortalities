namespace WildlifeMortalities.Data.Entities;

public class Client
{
    public int Id { get; set; }
    public int EnvClientId { get; set; }
    public List<Licence> Licences { get; set; } = new();
}
