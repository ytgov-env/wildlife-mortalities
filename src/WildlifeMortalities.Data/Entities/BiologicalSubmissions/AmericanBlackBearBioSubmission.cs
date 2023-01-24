using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission : BioSubmission<AmericanBlackBearMortality>
{
    public AmericanBlackBearSkullCondition SkullCondition { get; set; }
    public int? SkullLengthMillimetres { get; set; }
    public int? SkullWidthMillimetres { get; set; }

    public AmericanBlackBearBioSubmission() { }

    public AmericanBlackBearBioSubmission(int mortalityId) : base(mortalityId) { }
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
    public void Configure(EntityTypeBuilder<AmericanBlackBearBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
