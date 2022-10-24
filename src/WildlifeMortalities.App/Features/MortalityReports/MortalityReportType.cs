using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.App.Features.MortalityReports;

public enum MortalityReportType
{
    [Display(Name = "Human-wildlife conflict")]
    Conflict,

    [Display(Name = "Hunting (individual)")]
    IndividualHunt,

    [Display(Name = "Hunting (outfitted)")]
    OutfitterGuidedHunt,

    [Display(Name = "Hunting (special guided)")]
    SpecialGuidedHunt,

    [Display(Name = "Trapping")] Trapped
}
