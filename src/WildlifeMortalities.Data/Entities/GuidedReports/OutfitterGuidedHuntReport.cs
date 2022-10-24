using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.GuidedReports;

public class OutfitterGuidedHuntReport
{
    public int Id { get; set; }
    public List<Client> Guides { get; set; } = null!;
    public OutfitterArea OutfitterArea { get; set; } = null!;
    public GuidedHuntResult Result { get; set; }
    public List<HuntedMortalityReport> HuntedMortalityReports { get; set; } = null!;
}
