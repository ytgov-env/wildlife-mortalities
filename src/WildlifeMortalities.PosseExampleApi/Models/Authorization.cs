namespace WildlifeMortalities.PosseExampleApi.Models;

public record Authorization(
    AuthorizationType Type,
    string EnvClientId,
    string Number,
    string? WildlifeActPermitConditions,
    string? SpecialGuideLicenceGuidedHunterEnvClientId,
    DateTimeOffset? ValidFromDateTime,
    DateTimeOffset? ValidToDateTime,
    DateTimeOffset LastModifiedDateTime
);

public enum AuthorizationType
{
    BigGameHuntingLicence_CanadianResident = 10,
    BigGameHuntingLicence_CanadianResidentSpecialGuided = 20,
    BigGameHuntingLicence_NonResident = 30,
    BigGameHuntingLicence_YukonResident = 40,
    BigGameHuntingLicence_YukonResidentSenior = 50,
    BigGameHuntingLicence_YukonResidentFirstNationsOrInuit = 60,
    BigGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior = 70,
    BigGameHuntingLicence_YukonResidentTrapper = 80,
    HuntingPermit_CaribouFortymileFall = 90,
    HuntingPermit_CaribouFortymileSummerPeriodOne = 100,
    HuntingPermit_CaribouFortymileSummerPeriodTwo = 110,
    HuntingPermit_CaribouFortymileSummerPeriodThree = 120,
    HuntingPermit_CaribouFortymileSummerPeriodFour = 130,
    HuntingPermit_CaribouFortymileSummerPeriodFive = 140,
    HuntingPermit_CaribouFortymileSummerPeriodSix = 150,
    HuntingPermit_CaribouFortymileSummerPeriodSeven = 160,
    HuntingPermit_CaribouFortymileSummerPeriodEight = 170,
    HuntingPermit_CaribouFortymileWinter = 180,
    HuntingPermit_CaribouHartRiver = 190,
    HuntingPermit_CaribouNelchina = 200,
    HuntingPermit_ElkAdaptive = 210,
    HuntingPermit_ElkAdaptiveFirstNations = 220,
    HuntingPermit_ElkExclusion = 230,
    HuntingPermit_MooseThreshold = 240,
    HuntingPermit_WoodBisonThreshold = 250,
    HuntingSeal_AmericanBlackBear = 260,
    HuntingSeal_Caribou = 270,
    HuntingSeal_Deer = 280,
    HuntingSeal_Elk = 290,
    HuntingSeal_GrizzlyBear = 300,
    HuntingSeal_Moose = 310,
    HuntingSeal_MountainGoat = 320,
    HuntingSeal_ThinhornSheep = 330,
    HuntingSeal_WoodBison = 340,
    OutfitterAssistantGuideLicence = 350,
    OutfitterChiefGuideLicence = 360,
    PhaHuntingPermit_Caribou = 370,
    PhaHuntingPermit_Deer = 380,
    PhaHuntingPermit_Elk = 390,
    PhaHuntingPermit_Moose = 400,
    PhaHuntingPermit_MountainGoat = 410,
    PhaHuntingPermit_ThinhornSheep = 420,
    PhaHuntingPermit_ThinhornSheepKluane = 430,
    SmallGameHuntingLicence_CanadianResident = 440,
    SmallGameHuntingLicence_CanadianResidentFirstNationsOrInuit = 450,
    SmallGameHuntingLicence_NonResident = 460,
    SmallGameHuntingLicence_NonResidentFirstNationsOrInuit = 470,
    SmallGameHuntingLicence_YukonResident = 480,
    SmallGameHuntingLicence_YukonResidentSenior = 490,
    SmallGameHuntingLicence_YukonResidentFirstNationsOrInuit = 500,
    SmallGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior = 510,
    SpecialGuideLicence = 520,
    TrappingLicence_AssistantTrapper = 530,
    TrappingLicence_AssistantTrapperSenior = 540,
    TrappingLicence_ConcessionHolder = 550,
    TrappingLicence_ConcessionHolderSenior = 560,
    TrappingLicence_GroupConcessionAreaMember = 570,
    TrappingLicence_GroupConcessionAreaMemberSenior = 580,
    WildlifeActPermit = 590
}
