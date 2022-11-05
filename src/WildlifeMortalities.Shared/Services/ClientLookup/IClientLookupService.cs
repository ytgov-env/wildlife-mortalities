using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services.ClientLookup;

public interface IClientLookupService
{
    Task<IEnumerable<Client>> SearchByLastName(string input);

    Task<IEnumerable<Client>> SearchByEnvClientId(string input);

    Task<IEnumerable<Authorization>> GetAuthorizationsByEnvClientId(string input);
    Task<IEnumerable<Authorization>> GetAuthorizationsByClientId(int clientId);

    Task<IEnumerable<UpdateDto>> GetUpdates(DateTime startDateTime, DateTime endDateTime);
}
