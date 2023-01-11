using FluentValidation;
using MudBlazor;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class SpecialGuidedHuntReportViewModel
{
    public DateRange? HuntingDateRange { get; set; }
    public Client? Guide { get; set; }
    public GuidedHuntResult? Result { get; set; }

    public List<HuntedMortalityReportViewModel> HuntedMortalityReportViewModels { get; set; } =
        new();
}

public class SpecialGuidedHuntReportViewModelValidator
    : AbstractValidator<SpecialGuidedHuntReportViewModel>
{
    public SpecialGuidedHuntReportViewModelValidator()
    {
        RuleFor(x => x.Guide).NotNull();
        RuleFor(x => x.Result).IsInEnum().NotNull();
    }
}
