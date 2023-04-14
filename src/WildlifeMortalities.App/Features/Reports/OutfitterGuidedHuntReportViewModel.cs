﻿using FluentValidation;
using MudBlazor;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Reports;

public class OutfitterGuidedHuntReportViewModel
{
    public OutfitterGuidedHuntReportViewModel() { }

    public OutfitterGuidedHuntReportViewModel(OutfitterGuidedHuntReport report)
    {
        HuntingDateRange.Start = report.HuntStartDate;
        HuntingDateRange.End = report.HuntEndDate;
        ChiefGuide = report.ChiefGuide;
        AssistantGuides = report.AssistantGuides;
        OutfitterArea = report.OutfitterArea;
        Result = report.Result;
        HuntedActivityViewModels = report.HuntedActivities
            .Select(x => new HuntedActivityViewModel(x, report))
            .ToList();
    }

    public DateRange HuntingDateRange { get; set; } = new();
    public Client? SelectedAssistantGuide { get; set; }
    public Client? ChiefGuide { get; set; }
    public List<Client> AssistantGuides { get; set; } = new();
    public OutfitterArea? OutfitterArea { get; set; }
    public GuidedHuntResult? Result { get; set; }

    public List<HuntedActivityViewModel> HuntedActivityViewModels { get; set; } = new();

    public OutfitterGuidedHuntReport GetReport(int personId)
    {
        // Clear hunted activities if the hunter wasn't successful
        if (Result is not GuidedHuntResult.WentHuntingAndKilledWildlife)
        {
            HuntedActivityViewModels.Clear();
        }

        return Result is not GuidedHuntResult.DidNotHunt
            ? new OutfitterGuidedHuntReport
            {
                HuntStartDate = (DateTime)HuntingDateRange!.Start!,
                HuntEndDate = (DateTime)HuntingDateRange.End!,
                ChiefGuideId = ChiefGuide!.Id,
                AssistantGuides = AssistantGuides,
                OutfitterArea = OutfitterArea!,
                Result = Result!.Value,
                ClientId = personId,
                HuntedActivities = HuntedActivityViewModels.Select(x => x.GetActivity()).ToList()
            }
            : new OutfitterGuidedHuntReport
            {
                ChiefGuideId = ChiefGuide!.Id,
                AssistantGuides = AssistantGuides,
                OutfitterArea = OutfitterArea!,
                Result = Result!.Value,
                ClientId = personId,
            };
    }
}

public class OutfitterGuidedHuntReportViewModelValidator
    : AbstractValidator<OutfitterGuidedHuntReportViewModel>
{
    public OutfitterGuidedHuntReportViewModelValidator()
    {
        RuleFor(x => x.ChiefGuide).NotNull();
        RuleFor(x => x.AssistantGuides).NotEmpty();
        RuleFor(x => x.OutfitterArea).NotNull();
        RuleFor(x => x.Result).IsInEnum().NotNull();
        RuleFor(x => x.HuntingDateRange)
            .NotNull()
            .When(x => x.Result is not GuidedHuntResult.DidNotHunt)
            .WithMessage("Please enter the hunting dates.");
        RuleFor(x => x.HuntingDateRange)
            .Must(x => x.End <= DateTimeOffset.Now)
            .When(x => x.Result is not GuidedHuntResult.DidNotHunt)
            .WithMessage("The hunting dates cannot be in the future.");
        RuleFor(x => x.HuntingDateRange)
            .Must(IsHuntingDateRangeWithinSameSeason)
            .When(x => x.Result is not GuidedHuntResult.DidNotHunt)
            .WithMessage("The hunting dates must occur within the same season.");
        RuleFor(x => x.HuntingDateRange)
            .Must(
                (model, _) =>
                    model.HuntedActivityViewModels.Any(
                        y =>
                            y.MortalityWithSpeciesSelectionViewModel.MortalityViewModel.DateOfDeath
                                > model.HuntingDateRange.End
                            || y.MortalityWithSpeciesSelectionViewModel
                                .MortalityViewModel
                                .DateOfDeath < model.HuntingDateRange.Start
                    ) == false
            )
            .WithMessage(
                "The date of death for each mortality must be between the specified hunting dates"
            );
        RuleFor(x => x.HuntedActivityViewModels)
            .NotEmpty()
            .When(x => x.Result is GuidedHuntResult.WentHuntingAndKilledWildlife)
            .WithMessage(
                x =>
                    $"Please add at least one mortality, or change the {nameof(x.Result).ToLower()}."
            );
    }

    private bool IsHuntingDateRangeWithinSameSeason(DateRange dateRange)
    {
        if (dateRange.Start == null || dateRange.End == null)
        {
            return false;
        }

        var start = dateRange.Start.Value;
        var end = dateRange.End.Value;

        var seasonStart =
            start.Month < 4 ? new DateTime(start.Year - 1, 4, 1) : new DateTime(start.Year, 4, 1);
        var seasonEnd = seasonStart.AddYears(1).AddDays(-1);

        return start >= seasonStart && end <= seasonEnd;
    }
}
