using System.ComponentModel.DataAnnotations;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class ThinhornSheepMortality : Mortality<ThinhornSheepMortality>, IHasBioSubmission
{
    public ThinhornSheepBodyColour BodyColour { get; set; }
    public ThinhornSheepTailColour TailColour { get; set; }
    public ThinhornSheepBioSubmission? BioSubmission { get; set; }

    public override Species Species => Species.ThinhornSheep;
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
