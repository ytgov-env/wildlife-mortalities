using FluentValidation;

namespace WildlifeMortalities.App.Features.HarvestReports;

public class SelectHarvestReportTypeViewModel
{
    public HarvestReportType HarvestReportType { get; set; } = HarvestReportType.Hunted;
}

public class SelectHarvestReportTypeViewModelValidator
    : AbstractValidator<SelectHarvestReportTypeViewModel>
{
    public SelectHarvestReportTypeViewModelValidator()
    {
        RuleFor(x => x.HarvestReportType).NotNull();
    }
}

public enum HarvestReportType
{
    Hunted,
    Outfitted,
    Trapped
}
