using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using FluentValidation;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using static WildlifeMortalities.App.Features.Shared.Mortalities.OrganicMaterialForBioSubmissionDialog;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public class OrganicMaterialForBioSubmissionDialogViewModel
{
    private readonly BioSubmission _bioSubmission;
    public DateTime DateReportSubmitted { get; }
    public string Comment
    {
        get => _bioSubmission.Comment;
        set => _bioSubmission.Comment = value;
    }
    public DateTime? DateSubmitted
    {
        get => _bioSubmission.DateSubmitted?.DateTime;
        set => _bioSubmission.DateSubmitted = value;
    }

    public bool CanBeAnalysed => _bioSubmission.CanBeAnalysed;

    public bool HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis =>
        _bioSubmission.HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis();

    public IEnumerable<BioSubmissionMaterialCheckbox> RequiredOrganicMaterial { get; set; } =
        Array.Empty<BioSubmissionMaterialCheckbox>();

    public OrganicMaterialForBioSubmissionDialogViewModel(
        BioSubmission bioSubmission,
        Report report
    )
    {
        _bioSubmission = bioSubmission;
        DateReportSubmitted = report.DateSubmitted.DateTime;
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
        RuleFor(x => x.DateSubmitted)
            .Must(
                (viewModel, dateSubmitted) =>
                    dateSubmitted!.Value.Date >= viewModel.DateReportSubmitted.Date
            )
            .When(x => x.RequiredOrganicMaterial.All(x => x.IsChecked == true))
            .WithMessage(
                "The bio submission date submitted cannot occur before the date the report was submitted."
            );
        RuleFor(x => x.DateSubmitted)
            .LessThanOrEqualTo(_ => DateTime.Now)
            .When(x => x.RequiredOrganicMaterial.All(x => x.IsChecked == true))
            .WithMessage("The bio submission date submitted cannot occur in the future.");
        RuleFor(x => x.Comment).MaximumLength(500);
        RuleForEach(x => x.RequiredOrganicMaterial)
            .ChildRules(rules => rules.RuleFor(x => x.IsChecked).NotNull().WithMessage("Required"));
    }
}
