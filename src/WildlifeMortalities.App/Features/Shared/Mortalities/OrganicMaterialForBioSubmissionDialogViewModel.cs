using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using FluentValidation;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using static WildlifeMortalities.App.Features.Shared.Mortalities.OrganicMaterialForBioSubmissionDialog;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public class OrganicMaterialForBioSubmissionDialogViewModel
{
    private readonly BioSubmission _bioSubmission;
    public string Comment
    {
        get => _bioSubmission.Comment;
        set => _bioSubmission.Comment = value;
    }

    public bool CanBeAnalysed => _bioSubmission.CanBeAnalysed;

    public bool HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis =>
        _bioSubmission.HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis();

    public IEnumerable<BioSubmissionMaterialCheckbox> RequiredOrganicMaterial { get; set; } =
        Array.Empty<BioSubmissionMaterialCheckbox>();

    public OrganicMaterialForBioSubmissionDialogViewModel(BioSubmission bioSubmission)
    {
        _bioSubmission = bioSubmission;
        Comment = bioSubmission.Comment;
        var type = bioSubmission.GetType();
        RequiredOrganicMaterial = type.GetProperties()
            .Select(
                x =>
                    new
                    {
                        info = x,
                        attribute = x.GetCustomAttributes(false)
                            .OfType<IsRequiredOrganicMaterialForBioSubmissionAttribute>()
                            .FirstOrDefault()
                    }
            )
            .Where(x => x.attribute != null)
            .Select(
                x =>
                    new BioSubmissionMaterialCheckbox(
                        x.attribute!.DisplayName,
                        x.info,
                        _bioSubmission
                    )
            )
            .ToArray();
    }
}

public class OrganicMaterialForBioSubmissionDialogViewModelValidator
    : AbstractValidator<OrganicMaterialForBioSubmissionDialogViewModel>
{
    public OrganicMaterialForBioSubmissionDialogViewModelValidator()
    {
        RuleFor(x => x.Comment).MaximumLength(500);
        RuleForEach(x => x.RequiredOrganicMaterial)
            .ChildRules(rules =>
            {
                rules.RuleFor(x => x.IsChecked).NotNull().WithMessage("Required");
            });
    }
}
