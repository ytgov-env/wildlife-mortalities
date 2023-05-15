using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WolverineBioSubmission : BioSubmission<WolverineMortality>, IHasFurbearerSeal
{
    public WolverineBioSubmission() { }

    public WolverineBioSubmission(WolverineMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Pelt")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    [Column($"{nameof(WolverineBioSubmission)}_{nameof(IsPeltProvided)}")]
    public bool? IsPeltProvided { get; set; }

    [Column($"{nameof(FurbearerSealNumber)}")]
    public string? FurbearerSealNumber { get; set; }

    public override bool CanBeAnalysed => true;
}

public class WolverineBioSubmissionConfig : IEntityTypeConfiguration<WolverineBioSubmission>
{
    public void Configure(EntityTypeBuilder<WolverineBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder
            .Property(x => x.MortalityId)
            .HasColumnName($"{builder.Metadata.ClrType.Name}_MortalityId");
    }
}
