using WildlifeMortalities.Data.Entities.Licences;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class Seal
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public HuntedSpecies Species { get; set; }
    public int LicenceId { get; set; }
    public HuntingLicence Licence { get; set; } = null!;
    public HuntedHarvestReport? HarvestReport { get; set; }
}
