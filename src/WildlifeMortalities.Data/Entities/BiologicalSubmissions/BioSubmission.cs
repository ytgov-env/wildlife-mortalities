using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public abstract class BioSubmission
{
    public int Id { get; set; }
    public BioSubmissionRequiredOrganicMaterialsStatus RequiredOrganicMaterialsStatus { get; set; }
    public BioSubmissionAnalysisStatus AnalysisStatus { get; set; }
    public DateTimeOffset? DateSubmitted { get; set; }
    public DateTimeOffset? DateModified { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Age? Age { get; set; }
    public abstract void ClearDependencies();
    public abstract bool HasSubmittedAllRequiredOrganicMaterial();
    public virtual bool CanBeAnalysed { get; }

    public virtual bool HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis() =>
        CanBeAnalysed
            ? throw new Exception(
                $"This species can be analysed, but is missing an override for {nameof(HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis)}"
            )
            : false;
}

public abstract class BioSubmission<T> : BioSubmission
    where T : Mortality
{
    protected BioSubmission() { }

    protected BioSubmission(T mortality)
    {
        RequiredOrganicMaterialsStatus = BioSubmissionRequiredOrganicMaterialsStatus.NotStarted;
        AnalysisStatus = CanBeAnalysed
            ? BioSubmissionAnalysisStatus.NotStarted
            : BioSubmissionAnalysisStatus.NotApplicable;
        Mortality = mortality;
    }

    public int MortalityId { get; set; }

    public T Mortality { get; set; }

    public override void ClearDependencies() => Mortality = null!;
}

public class BioSubmissionConfig : IEntityTypeConfiguration<BioSubmission>
{
    public void Configure(EntityTypeBuilder<BioSubmission> builder)
    {
        builder.OwnsOne(
            b => b.Age,
            a =>
            {
                a.Property(a => a.Confidence).IsRequired();
                a.WithOwner();
            }
        );
        builder.Ignore(b => b.CanBeAnalysed);
    }
}

public class Age
{
    public int Years { get; set; }
    public ConfidenceInAge? Confidence { get; set; }
}

public enum ConfidenceInAge
{
    [Display(Name = "Poor")]
    Poor = 10,

    [Display(Name = "Fair")]
    Fair = 20,

    [Display(Name = "Good")]
    Good = 30
}

public enum BioSubmissionRequiredOrganicMaterialsStatus
{
    [Display(Name = "Not started")]
    NotStarted = 10,

    [Display(Name = "Did not submit")]
    DidNotSubmit = 20,

    [Display(Name = "Submitted")]
    Submitted = 30
}

public enum BioSubmissionAnalysisStatus
{
    [Display(Name = "Not applicable")]
    NotApplicable = 10,

    [Display(Name = "Not started")]
    NotStarted = 20,

    [Display(Name = "Complete")]
    Complete = 30
}
