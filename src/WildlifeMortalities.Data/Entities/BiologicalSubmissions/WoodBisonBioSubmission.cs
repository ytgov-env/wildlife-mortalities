using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WoodBisonBioSubmission : BioSubmission<WoodBisonMortality>
{
    public WoodBisonBioSubmission() { }

    public WoodBisonBioSubmission(WoodBisonMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Incisor bar")]
    public bool? IsIncisorBarProvided { get; set; }

    public override bool HasSubmittedAllRequiredOrganicMaterial() => IsIncisorBarProvided == true;
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
