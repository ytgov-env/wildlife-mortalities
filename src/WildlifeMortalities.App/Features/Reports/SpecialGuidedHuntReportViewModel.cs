using FluentValidation;
using MudBlazor;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Reports;

public class SpecialGuidedHuntReportViewModel
{
    public DateRange HuntingDateRange { get; set; } = new();
    public Client? Guide { get; set; }
    public GuidedHuntResult? Result { get; set; }

    public List<HuntedActivityViewModel> HuntedActivityViewModels { get; set; } = new();

    public SpecialGuidedHuntReport GetReport(int personId)
    {
        // Clear hunted activities if the hunter wasn't successful
        if (Result is not GuidedHuntResult.WentHuntingAndKilledWildlife)
        {
            HuntedActivityViewModels.Clear();
        }

        return Result is not GuidedHuntResult.DidNotHunt
            ? new SpecialGuidedHuntReport
            {
                HuntStartDate = (DateTime)HuntingDateRange!.Start!,
                HuntEndDate = (DateTime)HuntingDateRange.End!,
                GuideId = Guide!.Id,
                Result = Result!.Value,
                ClientId = personId,
                HuntedActivities = HuntedActivityViewModels.Select(x => x.GetActivity()).ToList()
            }
            : new SpecialGuidedHuntReport
            {
                GuideId = Guide!.Id,
                Result = Result!.Value,
                ClientId = personId,
            };
    }
}

public class SpecialGuidedHuntReportViewModelValidator
    : AbstractValidator<SpecialGuidedHuntReportViewModel>
{
    public SpecialGuidedHuntReportViewModelValidator()
    {
        RuleFor(x => x.HuntingDateRange.Start)
            .NotNull()
            .WithMessage("Please enter the hunting dates");
        RuleFor(x => x.HuntingDateRange.End)
            .Must(x => x <= DateTimeOffset.Now)
            .When(x => x.Result is not GuidedHuntResult.DidNotHunt)
            .WithMessage("The hunting dates cannot be in the future.");
        RuleFor(x => x.Guide).NotNull();
        RuleFor(x => x.Result).IsInEnum().NotNull();
        RuleFor(x => x.HuntedActivityViewModels)
            .NotEmpty()
            .When(x => x.Result is GuidedHuntResult.WentHuntingAndKilledWildlife)
            .WithMessage(
                x =>
                    $"Please add at least one mortality, or change the {nameof(x.Result).ToLower()}."
            );
        // Todo: show validation message in activity card
        RuleForEach(report => report.HuntedActivityViewModels)
            .Must(
                (report, activity) =>
                    activity.MortalityWithSpeciesSelectionViewModel.MortalityViewModel.DateOfDeath
                        >= report.HuntingDateRange.Start
                    && activity
                        .MortalityWithSpeciesSelectionViewModel
                        .MortalityViewModel
                        .DateOfDeath <= report.HuntingDateRange.End
            )
            .WithMessage(
                "The date of death for each mortality must be between the specified hunting dates"
            );
    }
}
