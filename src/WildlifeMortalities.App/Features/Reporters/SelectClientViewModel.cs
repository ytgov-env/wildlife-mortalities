using FluentValidation;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Reporters;

public class SelectClientViewModel
{
    public Client? SelectedClient { get; set; } = default!;
}

public class SelectClientViewModelValidator : AbstractValidator<SelectClientViewModel>
{
    public SelectClientViewModelValidator() => RuleFor(x => x.SelectedClient).NotNull();
}
