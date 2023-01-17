using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WoodBisonBioSubmission : BioSubmission<WoodBisonMortality>
{
    public bool IsIncisorBarIncluded { get; set; }
}

public class WoodBisonBioSubmissionConfig : IEntityTypeConfiguration<WoodBisonBioSubmission>
{
    public void Configure(EntityTypeBuilder<WoodBisonBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
