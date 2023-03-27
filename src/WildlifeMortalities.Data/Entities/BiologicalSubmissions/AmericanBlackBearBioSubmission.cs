using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission : BioSubmission<AmericanBlackBearMortality>
{
    public AmericanBlackBearBioSubmission() { }

    public AmericanBlackBearBioSubmission(int mortalityId)
        : base(mortalityId) { }

    public AmericanBlackBearSkullCondition? SkullCondition { get; set; }
    public int? SkullLengthMillimetres { get; set; }
    public int? SkullWidthMillimetres { get; set; }
}

public enum AmericanBlackBearSkullCondition
{
    [Display(Name = "Destroyed")]
    Destroyed,

    [Display(Name = "No skull submitted")]
    NoSkullSubmitted,

    [Display(Name = "Flesh off")]
    FleshOff,

    [Display(Name = "Flesh on")]
    FleshOn,

    [Display(Name = "Skin on")]
    SkinOn
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
        builder.Property(a => a.SkullCondition).IsRequired();
        // AmericanBlackBearBioSubmission is the first entity in the BioSubmission hierarchy that EF Core looks at
        // when generating a migration (alphabetically),so it gets the column "MortalityId" (sans prefix) for the foreign key
        // to Mortality. The other entities in this hierarchy are assigned to a column that contains their name as the prefix.
        builder.HasIndex(x => x.MortalityId).HasFilter("[MortalityId] IS NOT NULL");
    }
}
