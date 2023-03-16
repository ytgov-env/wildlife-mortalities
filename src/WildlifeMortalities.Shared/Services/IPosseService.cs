using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Shared.Services;

public interface IPosseService
{
    Task<IEnumerable<(Authorization, string)>> GetAuthorizations(
        DateTimeOffset modifiedSinceDateTime,
        Dictionary<string, Client> clientMapper,
        AppDbContext context
    );
    Task<IEnumerable<(Client, IEnumerable<string>)>> GetClients(
        DateTimeOffset modifiedSinceDateTime
    );
}
