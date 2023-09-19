// ReSharper disable InconsistentNaming

namespace WildlifeMortalities.Shared.Services.Posse;

public record ClientDto(
    string EnvClientId,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    DateTimeOffset LastModifiedDateTime,
    IEnumerable<string> PreviousEnvClientIds,
    Uri StaffUiUrl
);
