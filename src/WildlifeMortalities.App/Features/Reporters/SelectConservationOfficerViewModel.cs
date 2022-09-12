using FluentValidation;
using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.App.Features.Reporters;

public class SelectConservationOfficerViewModel
{
    public ConservationOfficer SelectedConservationOfficer { get; set; }
}

public class SelectConservationOfficerViewModelValidator : AbstractValidator<SelectConservationOfficerViewModel>
{
    public SelectConservationOfficerViewModelValidator()
    {
        RuleFor(x => x.SelectedConservationOfficer).NotNull();
    }
}
