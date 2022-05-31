using FluentValidation;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Shared.Validators;

public class MortalityValidator<T> : AbstractValidator<Mortality>
{
    public MortalityValidator()
    {
#pragma warning disable RCS1146 // Use conditional access.
        RuleFor(m => m.Coordinates)
            .Must(c => c == null || c.IsValid)
            .WithMessage("The coordinates are not topologically valid");
#pragma warning restore RCS1146 // Use conditional access.
        RuleFor(m => m.ReporterId).NotNull();
    }
}
