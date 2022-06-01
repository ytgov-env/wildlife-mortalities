using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Shared.Validators;

public class MortalityValidator<T> : AbstractValidator<Mortality>
{
    public MortalityValidator()
    {
#pragma warning disable RCS1146 // Use conditional access.
        RuleFor(m => m.Latitude)
            .Must(l => l == null || (l > 58 && l < 71))
            .WithMessage("Latitude must be between 58°N and 71°N");
        RuleFor(m => m.Latitude)
            .NotNull()
            .When(m => m.Longitude is not null)
            .WithMessage("Latitude and Longitude must both be set");
        RuleFor(m => m.Longitude)
            .Must(l => l == null || (l > -143 && l < -121))
            .WithMessage("Longitude must be between 121°W and 143°W");
        RuleFor(m => m.Longitude)
            .NotNull()
            .When(m => m.Latitude is not null)
            .WithMessage("Latitude and Longitude must both be set");
#pragma warning restore RCS1146 // Use conditional access.
        RuleFor(m => m.ReporterId).NotNull();
        RuleFor(m => m.Sex)
            .IsInEnum()
            .Must(s => s != Sex.Uninitialized)
            .WithMessage("Sex must be set to Female, Male, or Unknown");
    }
}
