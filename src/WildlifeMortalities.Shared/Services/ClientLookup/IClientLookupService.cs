using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public interface IClientLookupService
{
    Task<IEnumerable<ClientDto>> SearchByLastName(string input);

    Task<IEnumerable<ClientDto>> SearchByEnvClientId(string input);

    Task<IEnumerable<AuthorizationDto>> GetAuthorizationsByEnvClientId(string input);

    Task<IEnumerable<UpdateDto>> GetUpdates(DateTime startDateTime, DateTime endDateTime);
}
