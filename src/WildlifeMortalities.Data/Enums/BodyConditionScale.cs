using System.ComponentModel.DataAnnotations;

namespace WildlifeMortalities.Data.Enums;

public enum BodyConditionScale
{
    [Display(Name = "Not recorded (paper HRBS)")]
    NotSpecified = -1,

    [Display(Name = "1 – Emaciated (no fat remaining, sharp spine, prominent ribs)")]
    OneEmaciated = 10,

    [Display(
        Name = "2 – Thin (little if any fat under skin, ribs can be easily felt, generally poor body condition)"
    )]
    TwoThin = 20,

    [Display(
        Name = "3 – Medium (some fat under skin, ribs can be felt, generally good body condition)"
    )]
    ThreeMedium = 30,

    [Display(Name = "4 – Fat (lots of fat under the skin, ribs are hard to feel)")]
    FourFat = 40,

    [Display(
        Name = "5 – Obese (usually only in animals that have had access to very rich food sources)"
    )]
    FiveObese = 50,

    [Display(Name = "Unsure")]
    Unsure = 60,

    [Display(Name = "Declined to answer")]
    DeclinedToAnswer = 70
}
