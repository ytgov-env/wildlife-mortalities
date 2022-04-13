namespace WildlifeMortalities.Data.Entities;

public class Seal
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public int LicenceId { get; set; }
    public Licence Licence { get; set; } = null!;
    public List<HuntedMortality> HuntedMortalities { get; set; } = new();
}
