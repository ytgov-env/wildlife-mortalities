using FluentValidation;
using MudBlazor;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class SpecialGuidedHuntReportViewModel
{
    public DateRange? HuntingDateRange { get; set; }
    public Client? Guide { get; set; }
    public GuidedHuntResult? Result { get; set; }

    public List<HuntedActivityViewModel> HuntedMortalityReportViewModels { get; set; } = new();

    public SpecialGuidedHuntReport GetReport(int personId)
    {
        // Clear mortality reports if the hunter wasn't successful
        if (Result is not GuidedHuntResult.WentHuntingAndKilledWildlife)
        {
            HuntedMortalityReportViewModels.Clear();
        }

        var report = new SpecialGuidedHuntReport
        {
            ClientId = personId,
            HuntedActivities = HuntedMortalityReportViewModels.Select(x => x.GetActivity()).ToList()
        };

        return report;
    }
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
