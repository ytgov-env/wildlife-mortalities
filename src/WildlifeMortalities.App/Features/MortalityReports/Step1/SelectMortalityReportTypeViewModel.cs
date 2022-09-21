using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class SelectMortalityReportTypeViewModel
{
    public MortalityReportType MortalityReportType { get; set; } = MortalityReportType.Hunted;
}

public class SelectMortalityReportTypeViewModelValidator
    : AbstractValidator<SelectMortalityReportTypeViewModel>
{
    public SelectMortalityReportTypeViewModelValidator()
    {
        RuleFor(x => x.MortalityReportType).NotNull();
    }
}

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
