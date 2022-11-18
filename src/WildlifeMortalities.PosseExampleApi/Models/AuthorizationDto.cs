namespace WildlifeMortalities.PosseExampleApi.Models;

public record AuthorizationDto(
    AuthorizationType Type,
    string EnvClientId,
    string Number,
    DateTimeOffset ActiveFromDateTime,
    DateTimeOffset ActiveToDateTime,
    DateTimeOffset LastModifiedDateTime
);

public enum AuthorizationType
{
    Uninitialized = 0,
    BigGameHuntingLicence,
    HuntingPermit,
    HuntingSeal,
    OutfitterAssistantGuideLicence,
    OutfitterChiefGuideLicence,
    PhaHuntingPermit,
    SmallGameHuntingLicence,
    SpecialGuideLicence,
    TrappingLicence,
}
