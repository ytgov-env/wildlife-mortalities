using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum GuidedHuntResult
{
    [Display(Name = "Did not hunt")] DidNotHunt = 10,

    [Display(Name = "Went hunting and didn't kill wildlife")]
    WentHuntingAndDidntKillWildlife = 20,

    [Display(Name = "Went hunting and killed wildlife")]
    WentHuntingAndKilledWildlife = 30
}
