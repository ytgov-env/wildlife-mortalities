using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class ThinhornSheepBioSubmission : BioSubmission<ThinhornSheepMortality>
{
    public ThinhornSheepBioSubmission()
    {
    }

    public ThinhornSheepBioSubmission(int mortalityId) : base(mortalityId)
    {
    }

    public HornMeasured HornMeasured { get; set; }
    public BroomedStatus BroomedStatus { get; set; }
    public string PlugNumber { get; set; } = string.Empty;

    public int? HornTotalLengthMillimetres { get; set; }
    public int? HornBaseCircumferenceMillimetres { get; set; }
    public int? HornTipSpreadMillimetres { get; set; }
    public int? HornLengthToThirdAnnulusMillimetres { get; set; }

    public List<HornMeasurementEntry> HornMeasurementEntries { get; set; }
}

public class HornMeasurementEntry
{
    public int Annulus { get; set; }
    public int LengthMillimetres { get; set; }
    public int CircumferenceMillimetres { get; set; }
}

public enum BroomedStatus
{
    [Display(Name = "Both horns broomed")] BothHornsBroomed = 10,

    [Display(Name = "Left horn broomed")] LeftHornBroomed = 20,

    [Display(Name = "Not broomed")] NotBroomed = 30,

    [Display(Name = "Right horn broomed")] RightHornBroomed = 40
}

public enum HornMeasured
{
    [Display(Name = "Left horn")] LeftHorn = 10,

    [Display(Name = "Right horn")] RightHorn = 20,

    [Display(Name = "Polled (hornless)")] NoHornToMeasure = 30
}

public class ThinhornSheepBioSubmissionConfig : IEntityTypeConfiguration<ThinhornSheepBioSubmission>
{
    public void Configure(EntityTypeBuilder<ThinhornSheepBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        builder.OwnsMany(
            t => t.HornMeasurementEntries,
            ownedNavigationBuilder => ownedNavigationBuilder.ToJson()
        );
    }
}
