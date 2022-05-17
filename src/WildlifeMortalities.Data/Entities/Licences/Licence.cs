using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.Licences;

public class Licence
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}
