// ReSharper disable InconsistentNaming

namespace WildlifeMortalities.Shared.Services.Posse;

public record AuthorizationDto(
    string Type,
    string EnvClientId,
    string Number,
    string? CustomWildlifeActPermitConditions,
    string? SpecialGuideLicenceGuidedHunterEnvClientId,
    string? PhaHuntingPermitHuntCode,
    string? RegisteredTrappingConcession,
    DateTimeOffset ValidFromDateTime,
    DateTimeOffset ValidToDateTime,
    DateTimeOffset LastModifiedDateTime,
    IEnumerable<string> OutfitterAreas
);
