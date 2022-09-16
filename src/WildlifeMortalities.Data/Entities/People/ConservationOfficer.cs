namespace WildlifeMortalities.Data.Entities.People;

public class ConservationOfficer : Person
{
    public string BadgeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
