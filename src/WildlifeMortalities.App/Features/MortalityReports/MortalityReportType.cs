using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.App.Features.MortalityReports;

public enum MortalityReportType
{
    [Display(Name = "Human-wildlife conflict")]
    Conflict,
    [Display(Name = "Hunting (individual)")]
    Hunted,
    [Display(Name = "Hunting (outfitted)")]
    Outfitted,
    [Display(Name = "Hunting (special guided)")]
    SpecialGuided,
    [Display(Name = "Trapping")]
    Trapped
}
