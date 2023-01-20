using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission : BioSubmission<AmericanBlackBearMortality>
{
    public string SkullCondition { get; set; } = string.Empty;
    public int SkullLengthMillimetres { get; set; }
    public int SkullHeightMillimetres { get; set; }

    public AmericanBlackBearBioSubmission() { }

    public AmericanBlackBearBioSubmission(int mortalityId) : base(mortalityId) { }
}

public class AmericanBlackBearBioSubmissionConfig
    : IEntityTypeConfiguration<AmericanBlackBearBioSubmission>
{
    public void Configure(EntityTypeBuilder<AmericanBlackBearBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
