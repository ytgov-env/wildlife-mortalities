using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class GrizzlyBearBioSubmission : BioSubmission<GrizzlyBearMortality>
{
    public GrizzlyBearBioSubmission() { }

    public GrizzlyBearBioSubmission(GrizzlyBearMortality mortality)
        : base(mortality) { }

    [Column($"{nameof(GrizzlyBearBioSubmission)}_{nameof(SkullCondition)}")]
    public GrizzlyBearSkullCondition? SkullCondition { get; set; }

    [Column($"{nameof(GrizzlyBearBioSubmission)}_{nameof(SkullLengthMillimetres)}")]
    public int? SkullLengthMillimetres { get; set; }

    [Column($"{nameof(GrizzlyBearBioSubmission)}_{nameof(SkullWidthMillimetres)}")]
    public int? SkullWidthMillimetres { get; set; }

    [Column($"{nameof(GrizzlyBearBioSubmission)}_{nameof(IsToothExtracted)}")]
    public bool? IsToothExtracted { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Evidence of sex is attached")]
    [Column($"{nameof(GrizzlyBearBioSubmission)}_{nameof(IsEvidenceOfSexAttached)}")]
    public bool? IsEvidenceOfSexAttached { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Skull")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    [Column($"{nameof(GrizzlyBearBioSubmission)}_{nameof(IsSkullProvided)}")]
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
            .ToTable(TableNameConstants.BioSubmissions)
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder
            .Property(x => x.MortalityId)
            .HasColumnName($"{builder.Metadata.ClrType.Name}_MortalityId");
    }
}
