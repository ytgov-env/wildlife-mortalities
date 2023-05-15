using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class ElkBioSubmission : BioSubmission<ElkMortality>
{
    public ElkBioSubmission() { }

    public ElkBioSubmission(ElkMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Hide")]
    [Column($"{nameof(ElkBioSubmission)}_{nameof(IsHideProvided)}")]
    public bool? IsHideProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Head")]
    [Column($"{nameof(ElkBioSubmission)}_{nameof(IsHeadProvided)}")]
    public bool? IsHeadProvided { get; set; }
}

public class ElkBioSubmissionConfig : IEntityTypeConfiguration<ElkBioSubmission>
{
    public void Configure(EntityTypeBuilder<ElkBioSubmission> builder)
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
