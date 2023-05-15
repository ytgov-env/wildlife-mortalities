using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WoodBisonMortality : Mortality, IHasBioSubmission
{
    [Column($"{nameof(WoodBisonMortality)}_{nameof(PregnancyStatus)}")]
    public PregnancyStatus? PregnancyStatus { get; set; }

    [Column($"{nameof(WoodBisonMortality)}_{nameof(IsWounded)}")]
    public bool IsWounded { get; set; }
    public WoodBisonBioSubmission? BioSubmission { get; set; }

    public override Species Species => Species.WoodBison;

    public BioSubmission CreateDefaultBioSubmission() => new WoodBisonBioSubmission(this);
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
