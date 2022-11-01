using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public class ClientLookupService : IClientLookupService
{
    public Task<IEnumerable<ClientDto>> SearchByEnvClientId(string input) =>
        throw new NotImplementedException();

    public Task<IEnumerable<AuthorizationDto>> GetAuthorizationsByEnvClientId(string input) => throw new NotImplementedException();
    public Task<IEnumerable<UpdateDto>> GetUpdates(DateTime startDateTime, DateTime endDateTime) => throw new NotImplementedException();

    public Task<IEnumerable<ClientDto>> SearchByLastName(string input) =>
        throw new NotImplementedException();
}
