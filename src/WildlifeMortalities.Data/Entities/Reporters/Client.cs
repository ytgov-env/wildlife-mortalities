using WildlifeMortalities.Data.Entities.Licences;

namespace WildlifeMortalities.Data.Entities.Reporters;

public class Client : Reporter
{
    public string EnvClientId { get; set; } = string.Empty;
    public List<Licence> Licences { get; set; } = new();
}
