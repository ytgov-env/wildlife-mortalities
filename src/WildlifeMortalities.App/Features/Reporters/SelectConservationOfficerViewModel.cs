using FluentValidation;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Reporters;

public class SelectConservationOfficerViewModel
{
    public ConservationOfficer? SelectedConservationOfficer { get; set; }
}

public class SelectConservationOfficerViewModelValidator
    : AbstractValidator<SelectConservationOfficerViewModel>
{
    public SelectConservationOfficerViewModelValidator() =>
        RuleFor(x => x.SelectedConservationOfficer).NotNull();
}
