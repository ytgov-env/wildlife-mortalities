using WildlifeMortalities.Data.Entities.Licences;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities;

public class TrappedHarvestReport : HarvestReport
{
    public List<Mortality> Mortalities { get; set; } = null!;
    public int TrappingLicenceId { get; set; }
    public TrappingLicence TrappingLicence { get; set; } = null!;
}
