using FluentValidation;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Reports;

public class TrappedReportViewModel : MortalityReportViewModel
{
    public TrappedReportViewModel() { }

    public TrappedReportViewModel(TrappedMortalitiesReport report)
        : base(report)
    {
        TrappedActivityViewModels = report.TrappedActivities
            .Select(x => new TrappedActivityViewModel(x, report))
            .ToList();
        RegisteredTrappingConcession = report.RegisteredTrappingConcession;
    }

    public List<TrappedActivityViewModel> TrappedActivityViewModels { get; set; } = new();
    public RegisteredTrappingConcession? RegisteredTrappingConcession { get; set; }

    public override TrappedMortalitiesReport GetReport(int personId)
    {
        var report = new TrappedMortalitiesReport
        {
            ClientId = personId,
            TrappedActivities = TrappedActivityViewModels.ConvertAll(x => x.GetActivity()),
            RegisteredTrappingConcession = RegisteredTrappingConcession!,
            Id = ReportId,
        };

        SetReportBaseValues(report);
        return report;
    }

    internal override void SpeciesChanged(int id, Species species)
    {
        var activity = TrappedActivityViewModels.FirstOrDefault(
            x => x.MortalityWithSpeciesSelectionViewModel.MortalityViewModel.Id == id
        );
        if (activity == null)
        {
            return;
        }

        activity.ResetSpecies(species);
    }
}

public class TrappedReportViewModelValidator
    : MortalityReportViewModelValidator<TrappedReportViewModel>
{
    public TrappedReportViewModelValidator()
    {
        RuleFor(x => x.RegisteredTrappingConcession).NotNull();
        RuleFor(x => x.TrappedActivityViewModels).NotEmpty();
    }
}
