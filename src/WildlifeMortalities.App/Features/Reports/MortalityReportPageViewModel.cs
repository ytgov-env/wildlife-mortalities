using FluentValidation;

namespace WildlifeMortalities.App.Features.Reports;

public class MortalityReportPageViewModel
{
    public MortalityReportType MortalityReportType { get; set; } =
        MortalityReportType.IndividualHunt;

    public IndividualHuntedMortalityReportViewModel? IndividualHuntedMortalityReportViewModel { get; set; } =
        new();

    public OutfitterGuidedHuntReportViewModel? OutfitterGuidedHuntReportViewModel { get; set; }
    public SpecialGuidedHuntReportViewModel? SpecialGuidedHuntReportViewModel { get; set; }
    public TrappedReportViewModel? TrappedReportViewModel { get; set; }
}

public class MortalityReportPageViewModelValidator : AbstractValidator<MortalityReportPageViewModel>
{
    public MortalityReportPageViewModelValidator()
    {
        RuleFor(x => x.MortalityReportType).NotEmpty();

        RuleFor(x => x.IndividualHuntedMortalityReportViewModel)
            .SetValidator(new IndividualHuntedMortalityReportViewModelValidator())
            .When(x => x.MortalityReportType == MortalityReportType.IndividualHunt);

        RuleFor(x => x.OutfitterGuidedHuntReportViewModel)
            .SetValidator(new OutfitterGuidedHuntReportViewModelValidator())
            .When(x => x.MortalityReportType == MortalityReportType.OutfitterGuidedHunt);

        RuleFor(x => x.SpecialGuidedHuntReportViewModel)
            .SetValidator(new SpecialGuidedHuntReportViewModelValidator())
            .When(x => x.MortalityReportType == MortalityReportType.SpecialGuidedHunt);

        RuleFor(x => x.TrappedReportViewModel)
            .SetValidator(new TrappedReportViewModelValidator())
            .When(x => x.MortalityReportType == MortalityReportType.Trapped);
    }
}
