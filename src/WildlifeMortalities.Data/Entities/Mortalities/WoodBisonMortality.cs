using System.ComponentModel.DataAnnotations;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WoodBisonMortality : Mortality, IHasBioSubmission
{
    public PregnancyStatus? PregnancyStatus { get; set; }
    public bool IsWounded { get; set; }
    public WoodBisonBioSubmission? BioSubmission { get; set; }

    public override Species Species => Species.WoodBison;
}

public enum PregnancyStatus
{
    [Display(Name = "False")]
    False = 10,

    [Display(Name = "True")]
    True = 20,

    [Display(Name = "Unknown")]
    Unknown = 30
}
