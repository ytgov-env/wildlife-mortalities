using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Shared.Validators;

public class HuntedMortalityReportValidator<T> : AbstractValidator<HuntedMortalityReport>
    where T : Mortality
{
    public HuntedMortalityReportValidator()
    {
        RuleFor(h => h.Mortality).NotNull().SetValidator(new MortalityValidator<T>());
        RuleFor(h => h.Authorization).NotNull();
    }
}
