using FluentValidation;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.App.Features.Reporters;

public class SelectClientViewModel
{
    public Client? SelectedClient { get; set; } = default!;
}

public class SelectClientViewModelValidator : AbstractValidator<SelectClientViewModel>
{
    public SelectClientViewModelValidator() => RuleFor(x => x.SelectedClient).NotNull();
}
