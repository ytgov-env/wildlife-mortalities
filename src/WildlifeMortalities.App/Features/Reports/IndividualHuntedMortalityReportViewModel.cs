using FluentValidation;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Reports;

public class IndividualHuntedMortalityReportViewModel : MortalityReportViewModel
{
    public IndividualHuntedMortalityReportViewModel() { }

    public IndividualHuntedMortalityReportViewModel(IndividualHuntedMortalityReport report)
        : base(report)
    {
        HuntedActivityViewModel = new HuntedActivityViewModel(report.Activity, report);
    }

    public HuntedActivityViewModel HuntedActivityViewModel { get; set; } = new();

    public override IndividualHuntedMortalityReport GetReport(int personId)
    {
        var report = new IndividualHuntedMortalityReport()
        {
            Id = ReportId,
            PersonId = personId,
            Activity = HuntedActivityViewModel.GetActivity(),
        };

        SetReportBaseValues(report);
        return report;
    }

    internal override void SpeciesChanged(int id, Species species)
    {
        HuntedActivityViewModel.ResetSpecies(species);
    }
}

public class IndividualHuntedMortalityReportViewModelValidator
    : MortalityReportViewModelValidator<IndividualHuntedMortalityReportViewModel>
{
    public IndividualHuntedMortalityReportViewModelValidator()
    {
        RuleFor(x => x.HuntedActivityViewModel)
            .SetValidator(new HuntedActivityViewModelValidator());
        RuleFor(x => x.DateSubmitted)
            .Must(
                (model, dateSubmitted) =>
                    (dateSubmitted ?? DateTimeOffset.Now)
                    >= model
                        .HuntedActivityViewModel
                        .MortalityWithSpeciesSelectionViewModel
                        .MortalityViewModel
                        .DateOfDeath
            )
            .WithMessage("Date submitted cannot occur before date of death.");
    }
}
