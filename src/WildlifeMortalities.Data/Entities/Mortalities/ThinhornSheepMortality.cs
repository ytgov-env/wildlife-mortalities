using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class ThinhornSheepMortality : Mortality<ThinhornSheepMortality>, IHasBioSubmission
{
    [Column($"{nameof(ThinhornSheepMortality)}_{nameof(BodyColour)}")]
    public ThinhornSheepBodyColour BodyColour { get; set; }

    [Column($"{nameof(ThinhornSheepMortality)}_{nameof(TailColour)}")]
    public ThinhornSheepTailColour TailColour { get; set; }
    public ThinhornSheepBioSubmission? BioSubmission { get; set; }

    public override Species Species => Species.ThinhornSheep;

    public BioSubmission CreateDefaultBioSubmission() => new ThinhornSheepBioSubmission(this);
}

public enum ThinhornSheepBodyColour
{
    [Display(Name = "Dark")]
    Dark = 10,

    [Display(Name = "Fannin")]
    Fannin = 20,

    [Display(Name = "White")]
    White = 30
}

public enum ThinhornSheepTailColour
{
    [Display(Name = "Dark")]
    Dark = 10,

    [Display(Name = "White")]
    White = 20
}
