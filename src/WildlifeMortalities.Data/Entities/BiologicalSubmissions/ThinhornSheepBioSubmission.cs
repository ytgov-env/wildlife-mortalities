using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class ThinhornSheepBioSubmission : BioSubmission<ThinhornSheepMortality>
{
    public HornMeasured HornMeasured { get; set; }
    public BroomedStatus BroomedStatus { get; set; }
    public string PlugNumber { get; set; } = string.Empty;

    public int? HornTotalLengthMillimetres { get; set; }
    public int? HornBaseCircumferenceMillimetres { get; set; }
    public int? HornTipSpreadMillimetres { get; set; }
    public int? HornLengthToThirdAnnulusMillimetres { get; set; }

    public int? HornTipToFirstLengthMillimetres { get; set; }
    public int? HornTipToSecondLengthMillimetres { get; set; }
    public int? HornTipToThirdLengthMillimetres { get; set; }
    public int? HornTipToFourthLengthMillimetres { get; set; }
    public int? HornTipToFifthLengthMillimetres { get; set; }
    public int? HornTipToSixthLengthMillimetres { get; set; }
    public int? HornTipToSeventhLengthMillimetres { get; set; }
    public int? HornTipToEighthLengthMillimetres { get; set; }
    public int? HornTipToNinthLengthMillimetres { get; set; }
    public int? HornTipToTenthLengthMillimetres { get; set; }
    public int? HornTipToEleventhLengthMillimetres { get; set; }
    public int? HornTipToTwelvthLengthMillimetres { get; set; }
    public int? HornTipToThirteenthLengthMillimetres { get; set; }
    public int? HornTipToFourteenthLengthMillimetres { get; set; }
    public int? HornTipToFifteenthLengthMillimetres { get; set; }
    public int? HornTipToSixteenthLengthMillimetres { get; set; }

    public int? HornTipToFirstCircumferenceMillimetres { get; set; }
    public int? HornTipToSecondCircumferenceMillimetres { get; set; }
    public int? HornTipToThirdCircumferenceMillimetres { get; set; }
    public int? HornTipToFourthCircumferenceMillimetres { get; set; }
    public int? HornTipToFifthCircumferenceMillimetres { get; set; }
    public int? HornTipToSixthCircumferenceMillimetres { get; set; }
    public int? HornTipToSeventhCircumferenceMillimetres { get; set; }
    public int? HornTipToEighthCircumferenceMillimetres { get; set; }
    public int? HornTipToNinthCircumferenceMillimetres { get; set; }
    public int? HornTipToTenthCircumferenceMillimetres { get; set; }
    public int? HornTipToEleventhCircumferenceMillimetres { get; set; }
    public int? HornTipToTwelvthCircumferenceMillimetres { get; set; }
    public int? HornTipToThirteenthCircumferenceMillimetres { get; set; }
    public int? HornTipToFourteenthCircumferenceMillimetres { get; set; }
    public int? HornTipToFifteenthCircumferenceMillimetres { get; set; }
    public int? HornTipToSixteenthCircumferenceMillimetres { get; set; }

    public ThinhornSheepBioSubmission() { }

    public ThinhornSheepBioSubmission(int mortalityId) : base(mortalityId) { }
}

public enum BroomedStatus
{
    [Display(Name = "Both horns broomed")]
    BothHornsBroomed = 10,

    [Display(Name = "Left horn broomed")]
    LeftHornBroomed = 20,

    [Display(Name = "Not broomed")]
    NotBroomed = 30,

    [Display(Name = "Right horn broomed")]
    RightHornBroomed = 40
}

public enum HornMeasured
{
    [Display(Name = "Left horn")]
    LeftHorn = 10,

    [Display(Name = "Right horn")]
    RightHorn = 20,

    [Display(Name = "Polled (hornless)")]
    NoHornToMeasure
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
