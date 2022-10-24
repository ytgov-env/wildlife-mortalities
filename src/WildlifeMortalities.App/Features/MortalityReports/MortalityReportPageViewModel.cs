using FluentValidation;
using WildlifeMortalities.Data.Entities;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class MortalityReportPageViewModel
{
    public MortalityReportType MortalityReportType { get; set; } =
        MortalityReportType.IndividualHunt;

    public HuntedMortalityReportViewModel? HuntedMortalityReportViewModel { get; set; } = new();

    public OutfitterGuidedHuntReportViewModel? OutfitterGuidedHuntReportViewModel { get; set; }
    public SpecialGuidedHuntReportViewModel? SpecialGuidedHuntReportViewModel { get; set; }
}

public class MortalityReportViewModelValidator : AbstractValidator<MortalityReportPageViewModel>
{
}

public class HuntedMortalityReportViewModel
{
    public string Landmark { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public MortalityViewModel MortalityViewModel { get; set; } = new();

    public HuntedMortalityReport GetReport(int personId) =>
        new()
        {
            Mortality = MortalityViewModel.GetMortality(),
            Landmark = Landmark,
            Comment = Comment,
            ClientId = personId
        };
}

public class HuntedMortalityReportViewModelValidator
    : AbstractValidator<HuntedMortalityReportViewModel>
{
    public HuntedMortalityReportViewModelValidator()
    {
        RuleFor(x => x.Landmark).NotNull();
        RuleFor(x => x.Comment)
            .Length(10, 1000)
            .When(x => string.IsNullOrEmpty(x.Comment) == false);

        RuleFor(x => x.MortalityViewModel)
            .NotNull()
            .SetInheritanceValidator(x =>
            {
                x.Add(new AmericanBlackBearMortalityViewModelValidator());

                x.Add(new MortalityViewModelValidator());
            });
    }
}
