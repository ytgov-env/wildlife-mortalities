using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WhiteTailedDeerBioSubmission : BioSubmission<WhiteTailedDeerMortality>
{
    public WhiteTailedDeerBioSubmission() { }

    public WhiteTailedDeerBioSubmission(WhiteTailedDeerMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Hide")]
    [Column($"{nameof(WhiteTailedDeerBioSubmission)}_{nameof(IsHideProvided)}")]
    public bool? IsHideProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Head")]
    [Column($"{nameof(WhiteTailedDeerBioSubmission)}_{nameof(IsHeadProvided)}")]
    public bool? IsHeadProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Antlers")]
    [Column($"{nameof(WhiteTailedDeerBioSubmission)}_{nameof(IsAntlersProvided)}")]
    public bool? IsAntlersProvided { get; set; }
}

public class WhiteTailedDeerBioSubmissionConfig
    : IEntityTypeConfiguration<WhiteTailedDeerBioSubmission>
{
    public void Configure(EntityTypeBuilder<WhiteTailedDeerBioSubmission> builder)
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
