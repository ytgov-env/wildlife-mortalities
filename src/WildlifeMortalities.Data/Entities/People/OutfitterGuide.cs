using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

namespace WildlifeMortalities.Data.Entities.People;

public class OutfitterGuide
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? ReportAsChiefGuideId { get; set; }
    public OutfitterGuidedHuntReport? ReportAsChiefGuide { get; set; }
    public int? ReportAsAssistantGuideId { get; set; }
    public OutfitterGuidedHuntReport? ReportAsAssistantGuide { get; set; }
}
