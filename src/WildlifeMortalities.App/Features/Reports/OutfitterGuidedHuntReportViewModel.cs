using FluentValidation;
using MudBlazor;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Reports;

public class OutfitterGuidedHuntReportViewModel
{
    public DateRange HuntingDateRange { get; set; } = new();
    public Client? SelectedGuide { get; set; }
    public List<Client> Guides { get; set; } = new();
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

        var report = new OutfitterGuidedHuntReport
        {
            HuntStartDate = (DateTime)HuntingDateRange!.Start!,
            HuntEndDate = (DateTime)HuntingDateRange.End!,
            Guides = Guides,
            OutfitterArea = OutfitterArea!,
            Result = Result!.Value,
            ClientId = personId,
            HuntedActivities = HuntedActivityViewModels.Select(x => x.GetActivity()).ToList()
        };

        return report;
    }
}

public class OutfitterGuidedHuntReportViewModelValidator
    : AbstractValidator<OutfitterGuidedHuntReportViewModel>
{
    public OutfitterGuidedHuntReportViewModelValidator()
    {
        RuleFor(x => x.HuntingDateRange.Start)
            .NotNull()
            .WithMessage("Please enter the hunting dates.");
        RuleFor(x => x.HuntingDateRange.End)
            .Must(x => x <= DateTimeOffset.Now)
            .WithMessage("The hunting dates cannot be in the future.");
        RuleFor(x => x.Guides).NotEmpty();
        RuleFor(x => x.OutfitterArea).NotNull();
        RuleFor(x => x.Result).IsInEnum().NotNull();
        RuleFor(x => x.HuntedActivityViewModels)
            .NotEmpty()
            .When(x => x.Result is GuidedHuntResult.WentHuntingAndKilledWildlife);
    }
}
