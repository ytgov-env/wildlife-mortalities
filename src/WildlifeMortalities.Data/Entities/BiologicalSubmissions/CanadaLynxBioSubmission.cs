using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class CanadaLynxBioSubmission : BioSubmission<CanadaLynxMortality>, IHasFurbearerSeal
{
    public CanadaLynxBioSubmission() { }

    public CanadaLynxBioSubmission(CanadaLynxMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Pelt")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    [Column($"{nameof(CanadaLynxBioSubmission)}_{nameof(IsPeltProvided)}")]
    public bool? IsPeltProvided { get; set; }

    [Column($"{nameof(CanadaLynxBioSubmission)}_{nameof(PeltLengthMillimetres)}")]
    public int? PeltLengthMillimetres { get; set; }

    [Column($"{nameof(CanadaLynxBioSubmission)}_{nameof(PeltWidthMillimetres)}")]
    public int? PeltWidthMillimetres { get; set; }

    [Column($"{nameof(FurbearerSealNumber)}")]
    public string? FurbearerSealNumber { get; set; }
    public override bool CanBeAnalysed => true;
}

public class CanadaLynxBioSubmissionConfig : IEntityTypeConfiguration<CanadaLynxBioSubmission>
{
    public void Configure(EntityTypeBuilder<CanadaLynxBioSubmission> builder)
    {
        builder
            .ToTable(TableNameConstants.BioSubmissions)
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder
            .Property(x => x.MortalityId)
            .HasColumnName($"{builder.Metadata.ClrType.Name}_MortalityId");
        builder.HasIndex(x => x.FurbearerSealNumber).IsUnique();
    }
}
