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
    Hunted,
    Outfitted,
    SpecialGuided,
    Trapped
}
