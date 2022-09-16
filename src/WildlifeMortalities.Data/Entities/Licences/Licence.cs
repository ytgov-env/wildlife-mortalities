using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Licences;

public abstract class Licence
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Season => $"{StartDate.Year}-{EndDate.Year}";
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}
