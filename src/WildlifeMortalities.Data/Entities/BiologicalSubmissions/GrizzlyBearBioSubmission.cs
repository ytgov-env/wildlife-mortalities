using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class GrizzlyBearBioSubmission : BioSubmission<GrizzlyBearMortality>
{
    public GrizzlyBearBioSubmission() { }

    public GrizzlyBearBioSubmission(int mortalityId)
        : base(mortalityId) { }

    public GrizzlyBearBioSubmission(GrizzlyBearMortality mortality)
        : base(mortality) { }

    public GrizzlyBearSkullCondition? SkullCondition { get; set; }
    public int? SkullLengthMillimetres { get; set; }
    public int? SkullWidthMillimetres { get; set; }
    public bool IsEvidenceOfSexAttached { get; set; }
}

public enum GrizzlyBearSkullCondition
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

public class GrizzlyBearBioSubmissionConfig : IEntityTypeConfiguration<GrizzlyBearBioSubmission>
{
    public void Configure(EntityTypeBuilder<GrizzlyBearBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(g => g.SkullCondition).IsRequired();
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter("[GrizzlyBearBioSubmission_MortalityId] IS NOT NULL");
    }
}
