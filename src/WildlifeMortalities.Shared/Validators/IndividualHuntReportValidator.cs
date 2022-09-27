using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Shared.Validators;

public class IndividualHuntReportValidator<T> : AbstractValidator<IndividualHuntReport>
    where T : Mortality
{
    public IndividualHuntReportValidator()
    {
        RuleFor(h => h.Mortality).NotNull().SetValidator(new MortalityValidator<T>());
        RuleFor(h => h.Seal).NotNull();
    }
}
