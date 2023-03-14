using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Data.Entities;

public class RegisteredTrappingConcession
{
    public int Id { get; set; }
    public string Area { get; set; } = string.Empty;
    public List<TrappingLicence> TrappingLicences { get; set; } = null!;
}
