using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public interface IClientLookupService
{
    Task<IEnumerable<ClientDto>> SearchByLastName(string input);

    Task<IEnumerable<ClientDto>> SearchByEnvClientId(string input);
}
