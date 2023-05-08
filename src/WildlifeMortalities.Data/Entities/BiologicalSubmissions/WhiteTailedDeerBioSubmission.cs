using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WhiteTailedDeerBioSubmission : BioSubmission<WhiteTailedDeerMortality>
{
    public WhiteTailedDeerBioSubmission() { }

    public WhiteTailedDeerBioSubmission(WhiteTailedDeerMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Hide")]
    public bool? IsHideProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Head")]
    public bool? IsHeadProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Antlers")]
    public bool? IsAntlersProvided { get; set; }
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
            .OnDelete(DeleteBehavior.ClientCascade);
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter($"[{nameof(WhiteTailedDeerBioSubmission)}_MortalityId] IS NOT NULL");
    }
}
