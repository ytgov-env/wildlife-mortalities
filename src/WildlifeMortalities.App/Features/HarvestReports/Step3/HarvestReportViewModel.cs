using FluentValidation;

namespace WildlifeMortalities.App.Features.HarvestReports;

public class HarvestReportViewModel
{
    public string Landmark { get; set; } = String.Empty;
    public string Comments { get; set; } = String.Empty;
}

public class HarvestReportViewModelValidator : AbstractValidator<HarvestReportViewModel>
{
    public HarvestReportViewModelValidator(HarvestReportType type)
    {
        RuleFor(x => x.Landmark).NotNull().When(x => type == HarvestReportType.Hunted);
        RuleFor(x => x.Comments)
            .Length(10, 1000)
            .When(x => String.IsNullOrEmpty(x.Comments) == false);
    }
}
