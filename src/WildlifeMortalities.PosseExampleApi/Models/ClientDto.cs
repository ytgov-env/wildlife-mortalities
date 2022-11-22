namespace WildlifeMortalities.PosseExampleApi.Models;

public record ClientDto(
    string EnvClientId,
    IEnumerable<string> PreviousEnvClientIds,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    DateTimeOffset LastModifiedDateTime
);
