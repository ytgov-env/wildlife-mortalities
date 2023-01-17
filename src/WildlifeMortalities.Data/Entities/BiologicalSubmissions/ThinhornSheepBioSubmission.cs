using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class ThinhornSheepBioSubmission : BioSubmission<ThinhornSheepMortality>
{
    public BroomedStatus BroomedStatus { get; set; }
    public int LengthToThirdAnnulusMillimetres { get; set; }
    public string PlugNumber { get; set; } = string.Empty;
}

public enum BroomedStatus
{
    [Display(Name = "Both horns broomed")]
    BothHornsBroomed,

    [Display(Name = "Left horn broomed")]
    LeftHornBroomed,

    [Display(Name = "Not broomed")]
    NotBroomed,

    [Display(Name = "Right horn broomed")]
    RightHornBroomed
}

public class ThinhornSheepBioSubmissionConfig : IEntityTypeConfiguration<ThinhornSheepBioSubmission>
{
    public void Configure(EntityTypeBuilder<ThinhornSheepBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
