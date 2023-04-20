using FluentValidation;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public class BioSubmissionValidator<T> : AbstractValidator<T>
    where T : BioSubmission
{
    public BioSubmissionValidator()
    {
        When(
            x => x.Age != null,
            () =>
            {
                RuleFor(x => x.Age!.Years).InclusiveBetween(1, 80);
                RuleFor(x => x.Age!.Confidence).NotNull().IsInEnum();
            }
        );
        RuleFor(x => x.Comment).MaximumLength(1000);
    }
}
