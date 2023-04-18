using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class CaribouBioSubmission : BioSubmission<CaribouMortality>
{
    public CaribouBioSubmission() { }

    public CaribouBioSubmission(CaribouMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Incisor bar")]
    public bool? IsIncisorBarProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Evidence of sex")]
    public bool? IsEvidenceOfSexAttached { get; set; }

    public override bool HasSubmittedAllRequiredOrganicMaterial() =>
        IsIncisorBarProvided == true && IsEvidenceOfSexAttached == true;
}

public class CaribouBioSubmissionConfig : IEntityTypeConfiguration<CaribouBioSubmission>
{
    public void Configure(EntityTypeBuilder<CaribouBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter($"[{nameof(CaribouBioSubmission)}_MortalityId] IS NOT NULL");
    }
}
