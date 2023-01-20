using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class GrizzlyBearBioSubmission : BioSubmission<GrizzlyBearMortality>
{
    public string SkullCondition { get; set; } = string.Empty;
    public int SkullLengthMillimetres { get; set; }
    public int SkullHeightMillimetres { get; set; }
    public bool IsEvidenceOfSexIncluded { get; set; }

    public GrizzlyBearBioSubmission() { }

    public GrizzlyBearBioSubmission(int mortalityId) : base(mortalityId) { }
}

public class GrizzlyBearBioSubmissionConfig : IEntityTypeConfiguration<GrizzlyBearBioSubmission>
{
    public void Configure(EntityTypeBuilder<GrizzlyBearBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
