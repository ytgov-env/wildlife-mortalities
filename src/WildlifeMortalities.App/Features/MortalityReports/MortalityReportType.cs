using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.App.Features.MortalityReports;

public enum MortalityReportType
{
    [Display(Name = "Hunting (Individual)")]
    Hunted,
    [Display(Name = "Hunting (Outfitted)")]
    Outfitted,
    [Display(Name = "Hunting (Special Guided)")]
    SpecialGuided,
    [Display(Name = "Trapping")]
    Trapped
}
