namespace WildlifeMortalities.Shared.Models;

public class ClientDto
{
    public int Id { get; set; }
    public string EnvClientId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}
