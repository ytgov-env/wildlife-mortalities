using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WhiteTailedDeerBioSubmission : BioSubmission<WhiteTailedDeerMortality>
{
    public bool IsHornIncluded { get; set; }
    public bool IsHeadIncluded { get; set; }
}

public class WhiteTailedDeerBioSubmissionConfig
    : IEntityTypeConfiguration<WhiteTailedDeerBioSubmission>
{
    public void Configure(EntityTypeBuilder<WhiteTailedDeerBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
