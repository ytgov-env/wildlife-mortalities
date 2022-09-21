using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Data.Entities.People;

public class Client : Person
{
    public string EnvClientId { get; set; } = string.Empty;
    public List<Authorization> Authorizations { get; set; } = new();
}
