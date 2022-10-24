using FluentValidation;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.App.Features.Reporters;

public class SelectClientViewModel
{
    public ClientDto SelectedClient { get; set; }
}

public class SelectClientViewModelValidator : AbstractValidator<SelectClientViewModel>
{
    public SelectClientViewModelValidator() => RuleFor(x => x.SelectedClient).NotNull();
}
