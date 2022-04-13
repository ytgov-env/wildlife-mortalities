namespace WildlifeMortalities.Data.Entities;

public class Client : Reporter
{
    public string EnvClientId { get; set; } = string.Empty;
    public List<Licence> Licences { get; set; } = new();
}
