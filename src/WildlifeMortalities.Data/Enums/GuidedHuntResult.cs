using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum GuidedHuntResult
{
    [Display(Name = "Did not hunt")] DidNotHunt = 10,

    [Display(Name = "Failed hunt")] FailedHunt = 20,

    [Display(Name = "Successful hunt")] SuccessfulHunt = 30
}
