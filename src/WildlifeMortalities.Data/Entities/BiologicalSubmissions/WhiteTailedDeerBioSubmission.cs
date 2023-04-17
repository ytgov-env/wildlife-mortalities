using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WhiteTailedDeerBioSubmission : BioSubmission<WhiteTailedDeerMortality>
{
    public WhiteTailedDeerBioSubmission() { }

    public WhiteTailedDeerBioSubmission(WhiteTailedDeerMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Horn")]
    public bool? IsHornProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Head")]
    public bool? IsHeadProvided { get; set; }

    public override bool HasSubmittedAllRequiredOrganicMaterial() =>
        IsHornProvided == true && IsHeadProvided == true;
}

public class WhiteTailedDeerBioSubmissionConfig
    : IEntityTypeConfiguration<WhiteTailedDeerBioSubmission>
{
    public void Configure(EntityTypeBuilder<WhiteTailedDeerBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter("[WhiteTailedDeerBioSubmission_MortalityId] IS NOT NULL");
    }
}
