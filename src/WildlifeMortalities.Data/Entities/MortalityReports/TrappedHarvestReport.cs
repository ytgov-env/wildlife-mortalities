using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities;

public class TrappedHarvestReport : MortalityReport
{
    public int TrappingLicenceId { get; set; }
    public TrappingLicence TrappingLicence { get; set; } = null!;
    public string Comments { get; set; } = "";

}
