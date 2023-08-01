﻿using FluentValidation;
using MudBlazor;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Reports;

public class OutfitterGuidedHuntReportViewModel : MortalityReportViewModel
{
    public OutfitterGuidedHuntReportViewModel() { }

    public OutfitterGuidedHuntReportViewModel(OutfitterGuidedHuntReport report)
        : base(report)
    {
        HuntingDateRange.Start = report.HuntStartDate;
        HuntingDateRange.End = report.HuntEndDate;
        ChiefGuide = report.ChiefGuide;
        AssistantGuides = report.AssistantGuides
            .Union(Enumerable.Range(0, 2).Select(_ => new OutfitterGuide()))
            .Take(2)
            .ToArray();
        OutfitterArea = report.OutfitterArea;
        Result = report.Result;
        HuntedActivityViewModels = report.HuntedActivities
            .Select(x => new HuntedActivityViewModel(x, report))
            .ToList();
    }

    public DateRange HuntingDateRange { get; set; } = new();
    public OutfitterGuide? SelectedAssistantGuide { get; set; }
    public OutfitterGuide? ChiefGuide { get; set; } = new();
    public OutfitterGuide[] AssistantGuides { get; } =
        new[] { new OutfitterGuide(), new OutfitterGuide() };
    public OutfitterArea? OutfitterArea { get; set; }
    public GuidedHuntResult? Result { get; set; }

    public List<HuntedActivityViewModel> HuntedActivityViewModels { get; set; } = new();

    public override OutfitterGuidedHuntReport GetReport(int personId)
    {
        // Clear hunted activities if the hunter wasn't successful
        if (Result is not GuidedHuntResult.WentHuntingAndKilledWildlife)
        {
            HuntedActivityViewModels.Clear();
        }

        var report = new OutfitterGuidedHuntReport
        {
            ChiefGuide = ChiefGuide!.Id == 0 ? ChiefGuide : null!,
            AssistantGuides = AssistantGuides.ToList(),
            OutfitterArea = OutfitterArea!,
            Result = Result!.Value,
            ClientId = personId,
            Id = ReportId,
        };

        if (Result is not GuidedHuntResult.DidNotHunt)
        {
            report.HuntStartDate = (DateTime)HuntingDateRange!.Start!;
            report.HuntEndDate = (DateTime)HuntingDateRange.End!;
            report.HuntedActivities = HuntedActivityViewModels
                .Select(x => x.GetActivity())
                .ToList();
        }

        SetReportBaseValues(report);
        return report;
    }

    internal override void SpeciesChanged(int id, Species species)
    {
        var activity = HuntedActivityViewModels.FirstOrDefault(
            x => x.MortalityWithSpeciesSelectionViewModel.MortalityViewModel.Id == id
        );
        if (activity == null)
        {
            return;
        }

        activity.ResetSpecies(species);
    }
}

public class OutfitterGuideValidator : AbstractValidator<OutfitterGuide?>
{
    public OutfitterGuideValidator(bool? isRequired = null)
    {
        if (isRequired == true)
        {
            When(
                x => x is not null,
                () =>
                {
                    if (isRequired == true)
                    {
                        RuleFor(x => x!.FirstName).NotEmpty();
                        RuleFor(x => x!.LastName).NotEmpty();
                    }
                    RuleFor(x => x!.FirstName)
                        .NotEmpty()
                        .When(x => !string.IsNullOrWhiteSpace(x!.LastName))
                        .WithMessage("First name cannot be empty when last name is not empty.");
                    RuleFor(x => x!.LastName)
                        .NotEmpty()
                        .When(x => !string.IsNullOrWhiteSpace(x!.FirstName))
                        .WithMessage("Last name cannot be empty when first name is not empty.");
                    ;
                }
            );
        }
    }
}

public class OutfitterGuidedHuntReportViewModelValidator
    : MortalityReportViewModelValidator<OutfitterGuidedHuntReportViewModel>
{
    public OutfitterGuidedHuntReportViewModelValidator()
    {
        RuleFor(x => x.ChiefGuide).NotNull().SetValidator((_) => new OutfitterGuideValidator(true));
        RuleForEach(x => x.AssistantGuides)
            .SetValidator((x, y) => new OutfitterGuideValidator(x.AssistantGuides.First() == y));

        RuleFor(x => x.OutfitterArea).NotNull();
        RuleFor(x => x.Result).IsInEnum().NotNull();
        RuleFor(x => x.HuntingDateRange)
            .NotNull()
            .When(x => x.Result is not GuidedHuntResult.DidNotHunt)
            .WithMessage("Please enter the hunting dates.");
        RuleFor(x => x.HuntingDateRange)
            .Must(
                (model, dateRange) =>
                    dateRange.End <= (model.DateSubmitted?.Date ?? DateTimeOffset.Now.Date)
            )
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
