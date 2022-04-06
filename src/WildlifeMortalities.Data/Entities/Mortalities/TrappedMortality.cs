using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class TrappedMortality : MortalityBase
{
    public Sex Sex { get; set; }
    public int Quantity { get; set; }
    public string KillType { get; set; } = string.Empty;
    public DateTime KillDate { get; set; }
    public int RegisteredTrappingConcession { get; set; }
    public int Licenceid { get; set; }
    public Licence Licence { get; set; } = null!;
    public int HarvestReportId { get; set; }
    public TrappedHarvestReport HarvestReport { get; set; } = null!;
}
