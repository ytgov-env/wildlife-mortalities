using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class AmericanBlackBearBioSubmission : BioSubmission<AmericanBlackBearMortality>
{
    public AmericanBlackBearBioSubmission() { }

    public AmericanBlackBearBioSubmission(int mortalityId) : base(mortalityId) { }

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
    }
}
