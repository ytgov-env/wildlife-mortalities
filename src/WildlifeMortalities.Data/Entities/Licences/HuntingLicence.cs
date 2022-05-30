namespace WildlifeMortalities.Data.Entities.Licences;

public class HuntingLicence : Licence
{
    public List<Seal> Seals { get; set; } = new();
}
