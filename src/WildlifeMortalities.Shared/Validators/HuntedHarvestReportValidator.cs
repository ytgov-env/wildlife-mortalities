using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Shared.Validators;

public class HuntedHarvestReportValidator<T> : AbstractValidator<HuntedHarvestReport>
    where T : Mortality
{
    public HuntedHarvestReportValidator()
    {
        RuleFor(h => h.Mortality).NotNull().SetValidator(new MortalityValidator<T>());
        RuleFor(h => h.TemporarySealNumber)
            .Must(
                (harvestReport, temporarySealNumber) =>
                    (temporarySealNumber is null && harvestReport.Seal is not null)
                    || (temporarySealNumber?.Length >= 5 && harvestReport.Seal is null)
            )
            .WithMessage(
                "The harvest report must be associated with a valid Seal, or have a temporary seal number"
            );
    }
}
