using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public abstract class BioSubmission
{
    public int Id { get; set; }
    public BioSubmissionRequiredOrganicMaterialsStatus RequiredOrganicMaterialsStatus
    {
        get;
        protected set;
    }
    public BioSubmissionAnalysisStatus AnalysisStatus { get; protected set; }
    public DateTimeOffset? DateSubmitted { get; set; }
    public DateTimeOffset? DateModified { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Age? Age { get; set; }
    public abstract void ClearDependencies();

    private bool?[] GetRequiredOrganicMaterialStates() =>
        GetOrganicMaterialBooleans<IsRequiredOrganicMaterialForBioSubmissionAttribute>();

    private bool?[] GetOrganicMaterialPrerequisitesForAnalysis() =>
        GetOrganicMaterialBooleans<IsPrerequisiteOrganicMaterialForBioSubmissionAnalysisAttribute>();

    private bool?[] GetOrganicMaterialBooleans<T>()
        where T : Attribute
    {
        var type = GetType();
        var properties = type.GetProperties();
        return properties
            .Where(p => p.GetCustomAttribute<T>() != null)
            .Select(p => (bool?)p.GetValue(this))
            .ToArray();
    }

    public void UpdateRequiredOrganicMaterialsStatus()
    {
        var organicMaterialStates = GetRequiredOrganicMaterialStates();
        var nullValueFound = organicMaterialStates.Any(x => x == null);
        if (nullValueFound)
        {
            throw new InvalidOperationException(
                $"Bio submission {Id} was updated while a required organic material boolean was still null."
            );
        }

        var submittedMaterials = organicMaterialStates.Where(x => x!.Value).ToArray();
        if (submittedMaterials.Any())
        {
            if (organicMaterialStates.Length == 1)
            {
                RequiredOrganicMaterialsStatus =
                    BioSubmissionRequiredOrganicMaterialsStatus.Submitted;
            }
            else
            {
                RequiredOrganicMaterialsStatus =
                    submittedMaterials.Length == organicMaterialStates.Length
                        ? BioSubmissionRequiredOrganicMaterialsStatus.Submitted
                        : BioSubmissionRequiredOrganicMaterialsStatus.PartiallySubmitted;
            }
        }
        else
        {
            RequiredOrganicMaterialsStatus =
                BioSubmissionRequiredOrganicMaterialsStatus.DidNotSubmit;
        }
    }

    public void UpdateAnalysisStatus()
    {
        if (!HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis())
        {
            throw new InvalidOperationException(
                $"The analysis status for submission {Id} cannot be set to complete unless all of the prerequisite organic materials were provided."
            );
        }
        AnalysisStatus = BioSubmissionAnalysisStatus.Complete;
    }

    public virtual bool CanBeAnalysed { get; }

    public bool HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis()
    {
        if (CanBeAnalysed == false)
        {
            throw new Exception(
                $"This species can be analysed, but is missing an override for {nameof(HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis)}"
            );
        }

        var submissionValues = GetRequiredOrganicMaterialStates();
        if (submissionValues.All(x => x == null))
        {
            return false;
        }
        else if (submissionValues.Any(x => x == null))
        {
            throw new InvalidOperationException(
                $"Bio submission {Id} was updated while an organic material boolean is still null."
            );
        }

        var isAllSubmitted = !submissionValues.Any(x => x == false);
        return isAllSubmitted;
    }
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

    [Display(Name = "Partially submitted")]
    PartiallySubmitted = 30,

    [Display(Name = "Submitted")]
    Submitted = 40
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
