namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingLicence : Authorization
{
    public List<Seal> Seals { get; set; } = null!;
}
