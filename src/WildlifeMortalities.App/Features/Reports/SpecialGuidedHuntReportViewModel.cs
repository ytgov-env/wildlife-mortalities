using FluentValidation;
using MudBlazor;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public class SpecialGuidedHuntReportViewModel : MortalityReportViewModel
{
    public SpecialGuidedHuntReportViewModel() { }

    public SpecialGuidedHuntReportViewModel(
        SpecialGuidedHuntReport report,
        ReportDetail reportDetail
    )
        : base(report)
    {
        HuntingDateRange.Start = report.HuntStartDate;
        HuntingDateRange.End = report.HuntEndDate;
        Guide = report.Guide;
        Result = report.Result;
        OheNumber = report.OheNumber;
        HuntedActivityViewModels = report.HuntedActivities
            .Select(x => new HuntedActivityViewModel(x, reportDetail))
            .ToList();
    }

    public DateRange HuntingDateRange { get; set; } = new();
    public Client? Guide { get; set; }
    public GuidedHuntResult? Result { get; set; }
    public string OheNumber { get; set; } = string.Empty;

    public List<HuntedActivityViewModel> HuntedActivityViewModels { get; set; } = new();

    public override SpecialGuidedHuntReport GetReport(int personId)
    {
        // Clear hunted activities if the hunter wasn't successful
        if (Result is not GuidedHuntResult.WentHuntingAndKilledWildlife)
        {
            HuntedActivityViewModels.Clear();
        }

        var report = new SpecialGuidedHuntReport
        {
            Id = ReportId,
            GuideId = Guide!.Id,
            ClientId = personId,
            Result = Result!.Value,
            OheNumber = OheNumber,
        };

        if (Result is not GuidedHuntResult.DidNotHunt)
        {
            report.HuntStartDate = (DateTime)HuntingDateRange!.Start!;
            report.HuntEndDate = (DateTime)HuntingDateRange.End!;
            report.HuntedActivities = HuntedActivityViewModels
                .ConvertAll(x => x.GetActivity());
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

public class SpecialGuidedHuntReportViewModelValidator
    : MortalityReportViewModelValidator<SpecialGuidedHuntReportViewModel>
{
    public SpecialGuidedHuntReportViewModelValidator()
    {
        RuleFor(x => x.Guide).NotNull();
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
        RuleFor(x => x.OheNumber)
            .NotNull()
            .Matches(@"^\d{5}$")
            .WithMessage("OHE number must be exactly 5 digits.");
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
