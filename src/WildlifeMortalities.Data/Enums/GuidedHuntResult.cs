using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum GuidedHuntResult
{
    Uninitialized = 0,

    [Display(Name = "Did not hunt")]
    DidNotHunt = 1,

    [Display(Name = "Failed hunt")]
    FailedHunt = 2,

    [Display(Name = "Successful hunt")]
    SuccessfulHunt = 3,
}
