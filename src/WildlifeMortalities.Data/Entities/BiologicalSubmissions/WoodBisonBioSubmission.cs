using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WoodBisonBioSubmission : BioSubmission<WoodBisonMortality>
{
    public WoodBisonBioSubmission() { }

    public WoodBisonBioSubmission(int mortalityId)
        : base(mortalityId) { }

    public bool IsIncisorBarIncluded { get; set; }
}

public class WoodBisonBioSubmissionConfig : IEntityTypeConfiguration<WoodBisonBioSubmission>
{
    public void Configure(EntityTypeBuilder<WoodBisonBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter("[WoodBisonBioSubmission_MortalityId] IS NOT NULL");
    }
}
