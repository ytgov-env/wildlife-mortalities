using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public class DummyClientLookupService : IClientLookupService
{
    private List<ClientDto> _list = new List<ClientDto>
    {
        new ClientDto
        {
            Id = 1,
            LastName = "Doe",
            FirstName = "John",
            EnvClientId = "11124"
        },
        new ClientDto
        {
            Id = 2,
            LastName = "Doe",
            FirstName = "Jane",
            EnvClientId = "13030"
        },
        new ClientDto
        {
            Id = 3,
            LastName = "Bruce",
            FirstName = "Bill",
            EnvClientId = "14235"
        },
        new ClientDto
        {
            Id = 4,
            LastName = "Smith",
            FirstName = "Tom",
            EnvClientId = "19912"
        },
    };

    public Task<IEnumerable<ClientDto>> SearchByEnvClientId(string input) =>
        Task.FromResult(
            _list.Where(
                x =>
                    string.IsNullOrEmpty(input) == true
                        ? true
                        : x.EnvClientId.ToLower().StartsWith(input)
            )
        );

    public Task<IEnumerable<ClientDto>> SearchByLastName(string input) =>
        Task.FromResult(
            _list.Where(
                x =>
                    string.IsNullOrEmpty(input) == true
                        ? true
                        : x.LastName.ToLower().Contains(input)
            )
        );
}
