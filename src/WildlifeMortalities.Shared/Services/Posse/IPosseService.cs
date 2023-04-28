using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Shared.Services.Posse;

public interface IPosseService
{
    Task<IEnumerable<Authorization>> GetAuthorizations(
        DateTimeOffset modifiedSinceDateTime,
        Dictionary<string, PersonWithAuthorizations> personMapper,
        AppDbContext context
    );
    Task<IEnumerable<AuthorizationDto>> GetAuthorizationsByEnvClientId(
        string envClientId,
        DateTimeOffset modifiedSinceDateTime
    );
    Task<IEnumerable<(Client client, IEnumerable<string> previousEnvClientIds)>> GetClients(
        DateTimeOffset modifiedSinceDateTime
    );
}
