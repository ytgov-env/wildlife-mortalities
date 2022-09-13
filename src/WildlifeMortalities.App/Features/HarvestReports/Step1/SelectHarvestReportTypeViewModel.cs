using FluentValidation;

namespace WildlifeMortalities.App.Features.HarvestReports;

public class SelectHarvestReportTypeViewModel
{
    public HarvestReportType HarvestReportType { get; set; } = HarvestReportType.Hunting;
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
    Hunting,
    Trapping
}
