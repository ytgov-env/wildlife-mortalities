using WildlifeMortalities.Data.Entities.Licences;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities;

public class TrappedHarvestReport : HarvestReport
{
    public List<Mortality> Mortalities { get; set; } = new();
    public int TrappingLicenceId { get; set; }
    public TrappingLicence TrappingLicence { get; set; }
}
