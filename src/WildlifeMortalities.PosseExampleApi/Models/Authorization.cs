namespace WildlifeMortalities.PosseExampleApi.Models;

/// <param name="Type"></param>
/// <param name="EnvClientId"></param>
/// <param name="Number">The human-readable identifier for this authorization. (ex: EAT-0015403) </param>
/// <param name="CustomWildlifeActPermitConditions">
///     The conditions of a custom wildlife act permit. This is a placeholder
///     for now, as this authorization type is not yet implemented in POSSE.
/// </param>
/// <param name="SpecialGuideLicenceGuidedHunterEnvClientId">
///     The EnvClientId of the guided hunter. Null for every
///     authorization type except <see cref="AuthorizationType.SpecialGuideLicence" />.
/// </param>
/// <param name="PhaHuntingPermitHuntCode">The PHA hunt code. Null for every authorization type except PhaHuntingPermit_*</param>
/// <param name="OutfitterAreas">
///     The outfitter areas associated with this licence. Null for every authorization type except
///     <see cref="AuthorizationType.BigGameHuntingLicence_CanadianResident" />,
///     <see cref="AuthorizationType.SmallGameHuntingLicence_NonResident" />.
/// </param>
/// <param name="RegisteredTrappingConcession">
///     The registered trapping concession. Null for every authorization type except
///     TrappingLicence_*
/// </param>
/// <param name="ValidFromDateTime">The earliest datetime that the client can legally use this authorization.</param>
/// <param name="ValidToDateTime">The latest datetime that the client can legally use this authorization.</param>
/// <param name="LastModifiedDateTime">The most recent datetime that any of the values in this object were modified.</param>
public record Authorization(
    AuthorizationType Type,
    string EnvClientId,
    string Number,
    string? CustomWildlifeActPermitConditions,
    string? SpecialGuideLicenceGuidedHunterEnvClientId,
    string? PhaHuntingPermitHuntCode,
    IEnumerable<string> OutfitterAreas,
    string? RegisteredTrappingConcession,
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
    CustomWildlifeActPermit = 81,
    HuntingPermit_CaribouFortymileFall = 90,
    HuntingPermit_CaribouFortymileSummer = 100,
    HuntingPermit_CaribouFortymileWinter = 110,
    HuntingPermit_CaribouHartRiver = 120,
    HuntingPermit_CaribouNelchina = 130,
    HuntingPermit_ElkAdaptive = 140,
    HuntingPermit_ElkAdaptiveFirstNations = 150,
    HuntingPermit_ElkExclusion = 160,
    HuntingPermit_MooseThreshold = 170,
    HuntingPermit_WoodBisonThreshold = 180,
    HuntingSeal_AmericanBlackBear = 190,
    HuntingSeal_Caribou = 200,
    HuntingSeal_Deer = 210,
    HuntingSeal_Elk = 220,
    HuntingSeal_GrizzlyBear = 230,
    HuntingSeal_Moose = 240,
    HuntingSeal_MountainGoat = 250,
    HuntingSeal_ThinhornSheep = 260,
    HuntingSeal_WoodBison = 270,
    OutfitterAssistantGuideLicence = 280,
    OutfitterChiefGuideLicence = 290,
    PhaHuntingPermit_Caribou = 300,
    PhaHuntingPermit_Deer = 310,
    PhaHuntingPermit_Elk = 320,
    PhaHuntingPermit_Moose = 330,
    PhaHuntingPermit_MountainGoat = 340,
    PhaHuntingPermit_ThinhornSheep = 350,
    PhaHuntingPermit_ThinhornSheepKluane = 360,
    SmallGameHuntingLicence_CanadianResident = 370,
    SmallGameHuntingLicence_CanadianResidentFirstNationsOrInuit = 380,
    SmallGameHuntingLicence_NonResident = 390,
    SmallGameHuntingLicence_NonResidentFirstNationsOrInuit = 400,
    SmallGameHuntingLicence_YukonResident = 410,
    SmallGameHuntingLicence_YukonResidentSenior = 420,
    SmallGameHuntingLicence_YukonResidentFirstNationsOrInuit = 430,
    SmallGameHuntingLicence_YukonResidentFirstNationsOrInuitSenior = 440,
    SpecialGuideLicence = 450,
    TrappingLicence_AssistantTrapper = 460,
    TrappingLicence_AssistantTrapperSenior = 470,
    TrappingLicence_ConcessionHolder = 480,
    TrappingLicence_ConcessionHolderSenior = 490,
    TrappingLicence_GroupConcessionAreaMember = 500,
    TrappingLicence_GroupConcessionAreaMemberSenior = 510
}
