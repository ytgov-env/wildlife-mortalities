using FluentValidation;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

namespace WildlifeMortalities.App.Features.Reports;

public class TrappedReportViewModel
{
    public List<TrappedActivityViewModel> TrappedActivityViewModels { get; set; } = new();
    public RegisteredTrappingConcession? RegisteredTrappingConcession { get; set; }

    public TrappedMortalitiesReport GetReport(int personId)
    {
        return new TrappedMortalitiesReport
        {
            ClientId = personId,
            TrappedActivities = TrappedActivityViewModels.ConvertAll(x => x.GetActivity()),
            RegisteredTrappingConcession = RegisteredTrappingConcession!
        };
    }
}

public class TrappedReportViewModelValidator : AbstractValidator<TrappedReportViewModel>
{
    public TrappedReportViewModelValidator()
    {
        RuleFor(x => x.RegisteredTrappingConcession).NotNull();
        RuleFor(x => x.TrappedActivityViewModels).NotEmpty();
    }
}
