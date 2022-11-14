namespace WildlifeMortalities.PosseExampleApi.Models;

public record ClientDto(
    string EnvClientId,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    DateTime lastUpdatedDateTime
);
