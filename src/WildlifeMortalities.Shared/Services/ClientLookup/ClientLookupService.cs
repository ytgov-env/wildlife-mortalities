using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public class ClientLookupService : IClientLookupService
{
    public Task<IEnumerable<ClientDto>> SearchByEnvClientId(string input) =>
        throw new NotImplementedException();

    public Task<IEnumerable<ClientDto>> SearchByLastName(string input) =>
        throw new NotImplementedException();
}
