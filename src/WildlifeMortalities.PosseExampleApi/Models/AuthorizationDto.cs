namespace WildlifeMortalities.PosseExampleApi.Models;

public record AuthorizationDto(
    AuthorizationType Type,
    string EnvClientId,
    string Number,
    DateTimeOffset ActiveFromDateTime,
    DateTimeOffset ActiveToDateTime,
    IEnumerable<SealDto> Seals,
    DateTimeOffset lastUpdatedDateTime
);

public enum AuthorizationType
{
    Uninitialized = 0,
    HuntingLicence = 1,
    HuntingPermit = 2,
    PhaHuntingPermit = 3,
    SpecialGuideLicence = 4,
    TrappingLicence = 5
}
