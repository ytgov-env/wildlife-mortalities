using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class ThinhornSheepBioSubmission : BioSubmission
{
    public BroomedStatus BroomedStatus { get; set; }
    public int LengthToThirdAnnulusMillimetres { get; set; }
    public string PlugNumber { get; set; } = string.Empty;
}

public enum BroomedStatus
{
    [Display(Name = "Both horns broomed")] BothHornsBroomed,
    [Display(Name = "Left horn broomed")] LeftHornBroomed,
    [Display(Name = "Not broomed")] NotBroomed,
    [Display(Name = "Right horn broomed")] RightHornBroomed
}
