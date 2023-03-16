using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Shared.Services;

public interface IPosseService
{
    Task<IEnumerable<(Authorization authorization, string envClientId)>> GetAuthorizations(
        DateTimeOffset modifiedSinceDateTime,
        Dictionary<string, Client> clientMapper,
        AppDbContext context
    );
    Task<IEnumerable<PosseService.AuthorizationDto>> GetAuthorizationsByEnvClientId(
        string envClientId,
        DateTimeOffset modifiedSinceDateTime
    );
    Task<IEnumerable<(Client client, IEnumerable<string> previousEnvClientIds)>> GetClients(
        DateTimeOffset modifiedSinceDateTime
    );
}
