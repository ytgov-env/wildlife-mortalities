using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using FluentValidation;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.ThinhornSheep;

public partial class ThinhornSheepBioSubmissionComponent
{
    [Parameter]
    public bool IsReadOnly { get; set; }

    [Parameter]
    [EditorRequired]
    public ThinhornSheepBioSubmission BioSubmission { get; set; } = null!;

    [CascadingParameter]
    private EditContext Context { get; set; } = null!;

    [CascadingParameter(Name = Constants.CascadingValues.HasAttemptedFormSubmission)]
    public bool HasAttemptedFormSubmission { get; set; }

    protected override void OnInitialized()
    {
        BioSubmission.HornMeasurementEntries ??= new List<ThinhornSheepHornMeasurementEntry>();
        BioSubmission.Age ??= new Age();
    }

    private bool AnnuliCanBeBroomed()
    {
        return BioSubmission.BroomedStatus is BroomedStatus.BothHornsBroomed
            || (
                BioSubmission.BroomedStatus is BroomedStatus.LeftHornBroomed
                && BioSubmission.HornMeasured is HornMeasured.LeftHorn
            )
            || (
                BioSubmission.BroomedStatus is BroomedStatus.RightHornBroomed
                && BioSubmission.HornMeasured is HornMeasured.RightHorn
            );
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
            var hornMeasurementEntry = new ThinhornSheepHornMeasurementEntry
            {
                Annulus = BioSubmission.HornMeasurementEntries.Count + 1
            };
            BioSubmission.HornMeasurementEntries.Add(hornMeasurementEntry);
        }
    }

    private bool ShowAnnuliBroomedCheckbox(ThinhornSheepHornMeasurementEntry entry)
    {
        if (!AnnuliCanBeBroomed())
        {
            return false;
        }

        var entries = BioSubmission.HornMeasurementEntries;
        // Show checkbox if this is the first annulus
        if (entries.IndexOf(entry) == 0)
        {
            return true;
        }

        if (entries.Last() == entry)
        {
            return false;
        }

        // Show checkbox if the previous annulus was missing AND (this is the last annulus OR the annulus was marked as missing)
        return entries[BioSubmission.HornMeasurementEntries.IndexOf(entry) - 1].IsBroomed;
    }

    private bool IsAnnuliBroomedCheckboxDisabled(ThinhornSheepHornMeasurementEntry entry)
    {
        var entries = BioSubmission.HornMeasurementEntries;
        if (entries[0].Equals(entry))
        {
            return true;
        }

        var lastBroomedEntry = entries.Last(x => x.IsBroomed);
        var lastBroomedIndex = entries.IndexOf(lastBroomedEntry);
        var currentItemIndex = entries.IndexOf(entry);
        if (currentItemIndex == lastBroomedIndex && entry.IsBroomed)
        {
            return false;
        }

        if (currentItemIndex == lastBroomedIndex + 1)
        {
            return false;
        }

        return true;
    }

    private void ResetHornMeasurementEntryValues(ThinhornSheepHornMeasurementEntry entry)
    {
        entry.LengthMillimetres = 0;
        entry.CircumferenceMillimetres = 0;
        if (HasAttemptedFormSubmission)
        {
            Context.Validate();
        }
    }
}

public class ThinhornSheepBioSubmissionValidator
    : BioSubmissionValidator<ThinhornSheepBioSubmission>
{
    public ThinhornSheepBioSubmissionValidator()
    {
        RuleFor(x => x.IsBothEyeSocketsComplete).NotNull();
        RuleFor(x => x.HornMeasured).NotEmpty().IsInEnum();
        When(
            x => x.HornMeasured != null,
            () =>
            {
                RuleFor(x => x.Age).NotNull();
                RuleFor(x => x.BroomedStatus).NotEmpty().IsInEnum();
                RuleFor(x => x.PlugNumber).NotEmpty();
                RuleFor(x => x.HornLengthToThirdAnnulusMillimetres).GreaterThan(50).LessThan(1000);
                RuleFor(x => x.HornMeasurementEntries).NotEmpty();
                RuleFor(x => x.Age!.Years)
                    .Equal(x => x.HornMeasurementEntries.Last().Annulus)
                    .When(x => x.Age != null && x.HornMeasurementEntries.Any())
                    .WithMessage("Age must be equal to the number of annuli.");
                RuleForEach(x => x.HornMeasurementEntries)
                    .SetValidator(x => new ThinhornSheepHornMeasurementEntryValidator(x));
                RuleFor(x => x.HornTotalLengthMillimetres)
                    .Must((x, length) => length >= x.HornMeasurementEntries[^1].LengthMillimetres)
                    .When(x => x.HornMeasurementEntries.Count != 0)
                    .WithMessage(
                        "Total length must be greater than or equal to the length (from tip) of the last annulus."
                    );
                RuleFor(x => x.HornBaseCircumferenceMillimetres)
                    .Must(
                        (x, circumference) =>
                            circumference - x.HornMeasurementEntries[^1].CircumferenceMillimetres
                            >= -10
                    )
                    .When(x => x.HornMeasurementEntries.Count != 0)
                    .WithMessage(
                        "Base circumference can be max of 10mm less than the circumference of the last annulus."
                    );
                RuleFor(x => x.IsFullCurl).NotNull();
            }
        );
    }

    public class ThinhornSheepHornMeasurementEntryValidator
        : AbstractValidator<ThinhornSheepHornMeasurementEntry>
    {
        public ThinhornSheepHornMeasurementEntryValidator() { }

        public ThinhornSheepHornMeasurementEntryValidator(ThinhornSheepBioSubmission bioSubmission)
        {
            When(
                entry => !entry.IsBroomed,
                () =>
                {
                    RuleFor(entry => entry.LengthMillimetres)
                        .InclusiveBetween(10, 1000)
                        .GreaterThanOrEqualTo(
                            entry =>
                                bioSubmission.HornMeasurementEntries[
                                    bioSubmission.HornMeasurementEntries.IndexOf(entry) - 1
                                ].LengthMillimetres
                        )
                        .When(
                            entry =>
                                bioSubmission.HornMeasurementEntries.IndexOf(entry) != 0
                                && !bioSubmission.HornMeasurementEntries[
                                    bioSubmission.HornMeasurementEntries.IndexOf(entry) - 1
                                ].IsBroomed
                        )
                        .WithMessage(
                            "Length must be greater than or equal to the previous annulus."
                        );
                    RuleFor(entry => entry.CircumferenceMillimetres).GreaterThan(0);
                    RuleFor(entry => entry.CircumferenceMillimetres)
                        .Must(
                            (entry, circumference) =>
                                circumference
                                    - bioSubmission.HornMeasurementEntries[
                                        bioSubmission.HornMeasurementEntries.IndexOf(entry) - 1
                                    ].CircumferenceMillimetres
                                >= -10
                        )
                        .When(
                            (entry, _) =>
                                bioSubmission.HornMeasurementEntries.IndexOf(entry) != 0
                                && !bioSubmission.HornMeasurementEntries[
                                    bioSubmission.HornMeasurementEntries.IndexOf(entry) - 1
                                ].IsBroomed
                        )
                        .WithMessage(
                            "Circumference can be a max of 10mm less than the previous annulus."
                        );
                }
            );
        }
    }
}
