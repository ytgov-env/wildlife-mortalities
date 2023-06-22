using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission : BioSubmission<AmericanBlackBearMortality>
{
    public AmericanBlackBearBioSubmission() { }

    public AmericanBlackBearBioSubmission(AmericanBlackBearMortality mortality)
        : base(mortality) { }

    [Column($"{nameof(AmericanBlackBearBioSubmission)}_{nameof(SkullCondition)}")]
    public AmericanBlackBearSkullCondition? SkullCondition { get; set; }

    [Column($"{nameof(AmericanBlackBearBioSubmission)}_{nameof(SkullLengthMillimetres)}")]
    public int? SkullLengthMillimetres { get; set; }

    [Column($"{nameof(AmericanBlackBearBioSubmission)}_{nameof(SkullWidthMillimetres)}")]
    public int? SkullWidthMillimetres { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Skull")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    [Column($"{nameof(AmericanBlackBearBioSubmission)}_{nameof(IsSkullProvided)}")]
    public bool? IsSkullProvided { get; set; }

    public override bool CanBeAnalysed => true;

    public enum AmericanBlackBearSkullCondition
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

public class AmericanBlackBearBioSubmissionConfig
    : IEntityTypeConfiguration<AmericanBlackBearBioSubmission>
{
    public void Configure(EntityTypeBuilder<AmericanBlackBearBioSubmission> builder)
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
