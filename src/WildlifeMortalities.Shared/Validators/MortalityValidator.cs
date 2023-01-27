using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Shared.Validators;

public class MortalityValidator<T> : AbstractValidator<Mortality> where T : Mortality
{
    public MortalityValidator()
    {
        RuleFor(m => m.Latitude)
            .Must(latitude => latitude is null or > 58 and < 71)
            .WithMessage("Latitude must be between 58 and 71");
        RuleFor(m => m.Latitude)
            .Null()
            .When(m => m.Longitude is null)
            .WithMessage("Latitude cannot be set when longitude is null");

        RuleFor(m => m.Longitude)
            .Must(longitude => longitude is null or > -143 and < -121)
            .WithMessage("Longitude must be between -121 and -143");
        RuleFor(m => m.Longitude)
            .Null()
            .When(m => m.Latitude is null)
            .WithMessage("Longitude cannot be set when latitude is null");

        RuleFor(m => m.ReportId).NotNull();
        RuleFor(m => m.Sex)
            .IsInEnum()
            .Must(sex => sex != 0)
            .WithMessage("Sex must be set to Female, Male, or Unknown");
    }
}
