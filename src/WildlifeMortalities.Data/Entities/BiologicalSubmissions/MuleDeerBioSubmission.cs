using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class MuleDeerBioSubmission : BioSubmission<MuleDeerMortality>
{
    public bool IsHornIncluded { get; set; }
    public bool IsHeadIncluded { get; set; }

    public MuleDeerBioSubmission() { }

    public MuleDeerBioSubmission(int mortalityId) : base(mortalityId) { }
}

public class MuleDeerBioSubmissionConfig : IEntityTypeConfiguration<MuleDeerBioSubmission>
{
    public void Configure(EntityTypeBuilder<MuleDeerBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
