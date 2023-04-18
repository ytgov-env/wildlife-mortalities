using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class MooseBioSubmission : BioSubmission<MooseMortality>
{
    public MooseBioSubmission() { }

    public MooseBioSubmission(MooseMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Evidence of sex")]
    public bool? IsEvidenceOfSexAttached { get; set; }

    public override bool HasSubmittedAllRequiredOrganicMaterial() =>
        IsEvidenceOfSexAttached == true;
}

public class MooseBioSubmissionConfig : IEntityTypeConfiguration<MooseBioSubmission>
{
    public void Configure(EntityTypeBuilder<MooseBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter($"[{nameof(MooseBioSubmission)}_MortalityId] IS NOT NULL");
    }
}
