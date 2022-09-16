using WildlifeMortalities.Data.Entities.Licences;

namespace WildlifeMortalities.Data.Entities.People;

public class Client : Person
{
    public string EnvClientId { get; set; } = string.Empty;
    public List<Licence> Licences { get; set; } = new();
}
