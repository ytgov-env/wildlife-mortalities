using WildlifeMortalities.Data.Entities.Licences;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities;

public class TrappedHarvestReport : MortalityReport
{
    public List<Mortality> Mortalities { get; set; } = null!;
    public int TrappingLicenceId { get; set; }
    public TrappingLicence TrappingLicence { get; set; } = null!;
    public string Comments { get; set; } = "";

}
