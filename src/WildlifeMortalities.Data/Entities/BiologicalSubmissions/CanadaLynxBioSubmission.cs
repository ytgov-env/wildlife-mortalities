using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class CanadaLynxBioSubmission : BioSubmission<CanadaLynxMortality>, IHasFurbearerSeal
{
    public CanadaLynxBioSubmission() { }

    public CanadaLynxBioSubmission(CanadaLynxMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Pelt")]
    public bool? IsPeltProvided { get; set; }

    public int? PeltLengthMillimetres { get; set; }
    public int? PeltWidthMillimetres { get; set; }
    public string? FurbearerSealNumber { get; set; }

    public override bool HasSubmittedAllRequiredOrganicMaterial() => IsPeltProvided == true;
}

public class CanadaLynxBioSubmissionConfig : IEntityTypeConfiguration<CanadaLynxBioSubmission>
{
    public void Configure(EntityTypeBuilder<CanadaLynxBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter($"[{nameof(CanadaLynxBioSubmission)}_MortalityId] IS NOT NULL");
    }
}
