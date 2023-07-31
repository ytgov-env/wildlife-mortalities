﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
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

    private bool?[] GetRequiredOrganicMaterialValues() =>
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
        var organicMaterialValues = GetRequiredOrganicMaterialValues();
        var nullValueFound = organicMaterialValues.Any(x => x == null);
        if (nullValueFound)
        {
            throw new InvalidOperationException(
                $"Bio submission {Id} was updated while a required organic material boolean was still null."
            );
        }

        var submittedOrganicMaterials = organicMaterialValues.Where(x => x!.Value).ToArray();
        if (submittedOrganicMaterials.Any())
        {
            if (organicMaterialValues.Length == 1)
            {
                RequiredOrganicMaterialsStatus =
                    BioSubmissionRequiredOrganicMaterialsStatus.Submitted;
            }
            else
            {
                RequiredOrganicMaterialsStatus =
                    submittedOrganicMaterials.Length == organicMaterialValues.Length
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
                $"The {nameof(AnalysisStatus)} for submission {Id} cannot be set to {nameof(BioSubmissionAnalysisStatus.Complete)} unless all of the prerequisite organic materials were provided."
            );
        }
        AnalysisStatus = BioSubmissionAnalysisStatus.Complete;
    }

    [NotMapped]
    public virtual bool CanBeAnalysed { get; }

    public bool HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis()
    {
        if (CanBeAnalysed == false)
        {
            throw new InvalidOperationException(
                $"{nameof(HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis)} was called on a bio submission that does not require analysis."
            );
        }

        var prerequisiteOrganicMaterialValues = GetOrganicMaterialPrerequisitesForAnalysis();
        if (prerequisiteOrganicMaterialValues.Any(x => x == null))
        {
            return false;
        }

        return !prerequisiteOrganicMaterialValues.Any(x => x == false);
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
    [Display(Name = "N/A")]
    NotApplicable = 10,

    [Display(Name = "Not started")]
    NotStarted = 20,

    [Display(Name = "Complete")]
    Complete = 30
}
