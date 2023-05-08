using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class GrizzlyBearBioSubmission : BioSubmission<GrizzlyBearMortality>
{
    public GrizzlyBearBioSubmission() { }

    public GrizzlyBearBioSubmission(GrizzlyBearMortality mortality)
        : base(mortality) { }

    public GrizzlyBearSkullCondition? SkullCondition { get; set; }
    public int? SkullLengthMillimetres { get; set; }
    public int? SkullWidthMillimetres { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Evidence of sex is attached")]
    public bool? IsEvidenceOfSexAttached { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Skull")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    public bool? IsSkullProvided { get; set; }

    public override bool CanBeAnalysed => true;

    public enum GrizzlyBearSkullCondition
    {
        [Display(Name = "Destroyed")]
        Destroyed = 10,

        [Display(Name = "Flesh off")]
        FleshOff = 20,

        [Display(Name = "Flesh on")]
        FleshOn = 30,

        [Display(Name = "Skin on")]
        SkinOn = 40
    }
}

public class GrizzlyBearBioSubmissionConfig : IEntityTypeConfiguration<GrizzlyBearBioSubmission>
{
    public void Configure(EntityTypeBuilder<GrizzlyBearBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter($"[{nameof(GrizzlyBearBioSubmission)}_MortalityId] IS NOT NULL");
    }
}
