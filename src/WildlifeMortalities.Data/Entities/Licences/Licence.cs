using WildlifeMortalities.Data.Entities.Reporters;

namespace WildlifeMortalities.Data.Entities.Licences;

public class Licence
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Season => $"{StartDate.Year}-{EndDate.Year}";
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}
