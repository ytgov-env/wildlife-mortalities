using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WhiteTailedDeerBioSubmission : BioSubmission<WhiteTailedDeerMortality>
{
    public WhiteTailedDeerBioSubmission() { }

    public WhiteTailedDeerBioSubmission(int mortalityId)
        : base(mortalityId) { }

    public bool IsHornIncluded { get; set; }
    public bool IsHeadIncluded { get; set; }
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
