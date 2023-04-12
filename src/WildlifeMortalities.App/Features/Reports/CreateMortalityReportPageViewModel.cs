using FluentValidation;

namespace WildlifeMortalities.App.Features.Reports;

public class CreateMortalityReportPageViewModel
{
    public ReportType ReportType { get; set; } = ReportType.IndividualHuntedMortalityReport;

    public IndividualHuntedMortalityReportViewModel? IndividualHuntedMortalityReportViewModel { get; set; } =
        new();

    public OutfitterGuidedHuntReportViewModel? OutfitterGuidedHuntReportViewModel { get; set; }
    public SpecialGuidedHuntReportViewModel? SpecialGuidedHuntReportViewModel { get; set; }
    public TrappedReportViewModel? TrappedReportViewModel { get; set; }
}

public class CreateMortalityReportPageViewModelValidator
    : AbstractValidator<CreateMortalityReportPageViewModel>
{
    public CreateMortalityReportPageViewModelValidator()
    {
        RuleFor(x => x.ReportType).NotEmpty();

        RuleFor(x => x.IndividualHuntedMortalityReportViewModel)
            .SetValidator(new IndividualHuntedMortalityReportViewModelValidator())
            .When(x => x.ReportType == ReportType.IndividualHuntedMortalityReport);

        RuleFor(x => x.OutfitterGuidedHuntReportViewModel)
            .SetValidator(new OutfitterGuidedHuntReportViewModelValidator())
            .When(x => x.ReportType == ReportType.OutfitterGuidedHuntReport);

        RuleFor(x => x.SpecialGuidedHuntReportViewModel)
            .SetValidator(new SpecialGuidedHuntReportViewModelValidator())
            .When(x => x.ReportType == ReportType.SpecialGuidedHuntReport);

        RuleFor(x => x.TrappedReportViewModel)
            .SetValidator(new TrappedReportViewModelValidator())
            .When(x => x.ReportType == ReportType.TrappedMortalitiesReport);
    }
}
