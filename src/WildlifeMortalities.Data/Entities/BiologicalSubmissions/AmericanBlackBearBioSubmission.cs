using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission : BioSubmission<AmericanBlackBearMortality>
{
    public AmericanBlackBearBioSubmission() { }

    public AmericanBlackBearBioSubmission(AmericanBlackBearMortality mortality)
        : base(mortality) { }

    public AmericanBlackBearSkullCondition? SkullCondition { get; set; }
    public int? SkullLengthMillimetres { get; set; }
    public int? SkullWidthMillimetres { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Skull")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    public bool? IsSkullProvided { get; set; }

    public override bool CanBeAnalysed => true;

    public override bool HasSubmittedAllRequiredOrganicMaterial() => IsSkullProvided == true;

    public override bool HasSubmittedAllRequiredOrganicMaterialPrerequisitesForAnalysis() =>
        IsSkullProvided == true;

    public enum AmericanBlackBearSkullCondition
    {
        [Display(Name = "Destroyed")]
        Destroyed = 10,

        [Display(Name = "Flesh off")]
        FleshOff = 20,

        [Display(Name = "Flesh on")]
        FleshOn = 30,

        [Display(Name = "Skin on")]
        SkinOn = 40
    }
}

public class AmericanBlackBearBioSubmissionConfig
    : IEntityTypeConfiguration<AmericanBlackBearBioSubmission>
{
    public void Configure(EntityTypeBuilder<AmericanBlackBearBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        // AmericanBlackBearBioSubmission is the first entity in the BioSubmission hierarchy that EF Core looks at
        // when generating a migration (alphabetically),so it gets the column "MortalityId" (sans prefix) for the foreign key
        // to Mortality. The other entities in this hierarchy are assigned to a column that contains their name as the prefix.
        builder.HasIndex(x => x.MortalityId).HasFilter("[MortalityId] IS NOT NULL");
    }
}
