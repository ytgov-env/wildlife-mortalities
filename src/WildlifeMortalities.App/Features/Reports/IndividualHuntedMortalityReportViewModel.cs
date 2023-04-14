﻿using FluentValidation;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.App.Features.Reports;

public class IndividualHuntedMortalityReportViewModel
{
    public IndividualHuntedMortalityReportViewModel() { }

    public IndividualHuntedMortalityReportViewModel(IndividualHuntedMortalityReport report)
    {
        HuntedActivityViewModel = new HuntedActivityViewModel(report.HuntedActivity, report);
    }

    public HuntedActivityViewModel HuntedActivityViewModel { get; set; } = new();

    public IndividualHuntedMortalityReport GetReport(int personId) =>
        new()
        {
            HuntedActivity = HuntedActivityViewModel.GetActivity(),
            ClientId = personId
            // Todo add date logic
        };
}

public class IndividualHuntedMortalityReportViewModelValidator
    : AbstractValidator<IndividualHuntedMortalityReportViewModel>
{
    public IndividualHuntedMortalityReportViewModelValidator() =>
        RuleFor(x => x.HuntedActivityViewModel)
            .SetValidator(new HuntedActivityViewModelValidator());
}
