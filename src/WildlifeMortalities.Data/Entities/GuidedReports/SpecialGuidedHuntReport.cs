using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.GuideReports;

public class SpecialGuidedHuntReport
{
    public int Id { get; set; }
    public int GuideId { get; set; }
    public Client Guide { get; set; } = null!;
    public GuidedHuntResult Result { get; set; }
    public List<HuntedMortalityReport> HuntedMortalityReports { get; set; } = null!;
}
