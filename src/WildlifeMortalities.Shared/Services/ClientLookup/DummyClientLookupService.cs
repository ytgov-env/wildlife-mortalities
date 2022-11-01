using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public class DummyClientLookupService : IClientLookupService
{
    private readonly List<ClientDto> _list = new()
    {
        new ClientDto("45010", "Brad", "Gurp", new DateOnly(1993, 12, 12), DateTime.Today)
    };

    public Task<IEnumerable<ClientDto>> SearchByEnvClientId(string input) =>
        Task.FromResult(
            _list.Where(
                x =>
                    string.IsNullOrEmpty(input) || x.EnvClientId.ToLower().StartsWith(input)
            )
        );

    public Task<IEnumerable<AuthorizationDto>> GetAuthorizationsByEnvClientId(string input) => throw new NotImplementedException();
    public Task<IEnumerable<UpdateDto>> GetUpdates(DateTime startDateTime, DateTime endDateTime) => throw new NotImplementedException();

    public Task<IEnumerable<ClientDto>> SearchByLastName(string input) =>
        Task.FromResult(
            _list.Where(
                x =>
                    string.IsNullOrEmpty(input) || x.LastName.ToLower().Contains(input)
            )
        );
}
