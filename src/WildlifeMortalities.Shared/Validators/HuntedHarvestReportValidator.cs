using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WildlifeMortalities.Data.Entities;

namespace WildlifeMortalities.Shared.Validators;

public class HuntedHarvestReportValidator<T> : AbstractValidator<HuntedHarvestReport>
{
    public HuntedHarvestReportValidator()
    {
        RuleFor(h => h.Mortality).NotNull().SetValidator(new MortalityValidator<T>());
        RuleFor(h => h.TemporarySealNumber)
            .Must(
                (harvestReport, temporarySealNumber) =>
                    harvestReport.Seal == null || temporarySealNumber.Length >= 5
            )
            .WithMessage(
                "The harvest report must be associated with a valid Seal, or have a temporary seal number"
            );
    }
}
