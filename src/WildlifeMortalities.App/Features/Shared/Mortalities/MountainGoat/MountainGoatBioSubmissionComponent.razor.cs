using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.MountainGoat;

public partial class MountainGoatBioSubmissionComponent
{
    [Parameter]
    public bool IsReadOnly { get; set; }

    [Parameter]
    [EditorRequired]
    public MountainGoatBioSubmission BioSubmission { get; set; } = null!;

    [CascadingParameter(Name = Constants.CascadingValues.HasAttemptedFormSubmission)]
    public bool HasAttemptedFormSubmission { get; set; }

    protected override void OnInitialized()
    {
        BioSubmission.HornMeasurementEntries ??= new List<MountainGoatHornMeasurementEntry>();
        BioSubmission.Age ??= new Age();
    }

    private void AdjustAnnuliList()
    {
        while (BioSubmission.HornMeasurementEntries.Count > BioSubmission.Age!.Years)
        {
            BioSubmission.HornMeasurementEntries.RemoveAt(
                BioSubmission.HornMeasurementEntries.Count - 1
            );
        }

        while (BioSubmission.HornMeasurementEntries.Count < BioSubmission.Age.Years)
        {
            var hornMeasurementEntry = new MountainGoatHornMeasurementEntry
            {
                Annulus = BioSubmission.HornMeasurementEntries.Count + 1
            };
            BioSubmission.HornMeasurementEntries.Add(hornMeasurementEntry);
        }
    }
}

public class MountainGoatBioSubmissionValidator : BioSubmissionValidator<MountainGoatBioSubmission>
{
    public MountainGoatBioSubmissionValidator()
    {
        RuleFor(x => x.HornMeasured).NotEmpty().IsInEnum();
        When(
            x => x.HornMeasured != null,
            () =>
            {
                RuleFor(x => x.Age).NotNull();
                RuleFor(x => x.HornLengthToThirdAnnulusMillimetres).GreaterThan(50).LessThan(1000);
                RuleFor(x => x.HornMeasurementEntries).NotEmpty();
                RuleFor(x => x.Age!.Years)
                    .Equal(x => x.HornMeasurementEntries.Last().Annulus)
                    .When(x => x.Age != null && x.HornMeasurementEntries.Any())
                    .WithMessage("Age must be equal to the number of annuli.");
                RuleForEach(x => x.HornMeasurementEntries)
                    .SetValidator(x => new MountainGoatHornMeasurementEntryValidator(x));
                RuleFor(x => x.HornTotalLengthMillimetres)
                    .Must((x, length) => length >= x.HornMeasurementEntries[^1].LengthMillimetres)
                    .When(x => x.HornMeasurementEntries.Count != 0)
                    .WithMessage(
                        "Total length must be greater than or equal to the length (from tip) of the last annulus."
                    );
            }
        );
    }

    public class MountainGoatHornMeasurementEntryValidator
        : AbstractValidator<MountainGoatHornMeasurementEntry>
    {
        public MountainGoatHornMeasurementEntryValidator() { }

        public MountainGoatHornMeasurementEntryValidator(MountainGoatBioSubmission bioSubmission)
        {
            RuleFor(entry => entry.LengthMillimetres)
                .InclusiveBetween(10, 1000)
                .GreaterThanOrEqualTo(
                    entry =>
                        bioSubmission.HornMeasurementEntries[
                            bioSubmission.HornMeasurementEntries.IndexOf(entry) - 1
                        ].LengthMillimetres
                )
                .When(entry => bioSubmission.HornMeasurementEntries.IndexOf(entry) != 0)
                .WithMessage("Length must be greater than or equal to the previous annulus.");
        }
    }
}
