using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;

namespace WildlifeMortalities.Data.Entities;

public class RegisteredTrappingConcession
{
    public int Id { get; set; }
    public string Concession { get; set; } = string.Empty;
    public List<TrappingLicence> TrappingLicences { get; set; } = null!;
    public List<TrappingBagLimitEntry> TrappingBagLimitEntries { get; set; } = null!;
}
