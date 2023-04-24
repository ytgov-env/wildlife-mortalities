using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class ElkBioSubmission : BioSubmission<ElkMortality>
{
    public ElkBioSubmission() { }

    public ElkBioSubmission(ElkMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Hide")]
    public bool? IsHideProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Head")]
    public bool? IsHeadProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Evidence of sex is attached")]
    public bool? IsEvidenceOfSexAttached { get; set; }

    public override bool HasSubmittedAllRequiredOrganicMaterial() =>
        IsHideProvided == true && IsHeadProvided == true && IsEvidenceOfSexAttached == true;
}

public class ElkBioSubmissionConfig : IEntityTypeConfiguration<ElkBioSubmission>
{
    public void Configure(EntityTypeBuilder<ElkBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter("[ElkBioSubmission_MortalityId] IS NOT NULL");
    }
}
