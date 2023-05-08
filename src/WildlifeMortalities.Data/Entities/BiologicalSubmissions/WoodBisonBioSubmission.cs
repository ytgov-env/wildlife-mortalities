using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WoodBisonBioSubmission : BioSubmission<WoodBisonMortality>
{
    public WoodBisonBioSubmission() { }

    public WoodBisonBioSubmission(WoodBisonMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Incisor bar")]
    public bool? IsIncisorBarProvided { get; set; }
}

public class WoodBisonBioSubmissionConfig : IEntityTypeConfiguration<WoodBisonBioSubmission>
{
    public void Configure(EntityTypeBuilder<WoodBisonBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter($"[{nameof(WoodBisonBioSubmission)}_MortalityId] IS NOT NULL");
    }
}
