using FluentValidation;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.App.Features.Reports;

public class IndividualHuntedMortalityReportViewModel
{
    public HuntedActivityViewModel HuntedActivityViewModel { get; set; } = new();

    public IndividualHuntedMortalityReport GetReport(int personId)
    {
        return new IndividualHuntedMortalityReport()
        {
            HuntedActivity = HuntedActivityViewModel.GetActivity(),
            ClientId = personId
            // Todo add date logic
        };
    }
}

public class IndividualHuntedMortalityReportViewModelValidator
    : AbstractValidator<IndividualHuntedMortalityReportViewModel>
{
    public IndividualHuntedMortalityReportViewModelValidator()
    {
        RuleFor(x => x.HuntedActivityViewModel)
            .SetValidator(new HuntedActivityViewModelValidator());
    }
}
