using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public enum BroomedStatus
{
    [Display(Name = "Both horns broomed")] BothHornsBroomed = 10,

    [Display(Name = "Left horn broomed")] LeftHornBroomed = 20,

    [Display(Name = "Not broomed")] NotBroomed = 30,

    [Display(Name = "Right horn broomed")] RightHornBroomed = 40
}
