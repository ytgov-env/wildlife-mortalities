using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services.ClientLookup;

public class DummyClientLookupService : IClientLookupService
{
    private readonly List<Client> _list = new()
    {
        new Client
        {
            EnvClientId = "45010", FirstName = "Brad", LastName = "Gurp", BirthDate = new DateTime(1993, 12, 12)
        },
        new Client
        {
            EnvClientId = "30542",
            FirstName = "Tim",
            LastName = "Topper",
            BirthDate = new DateTime(1993, 12, 12)
        }
    };

    private readonly List<Authorization> _authorizations = new()
    {
        new HuntingLicence { ClientId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now, Number = "EHL-2520" }
    };


    public Task<IEnumerable<Client>> SearchByEnvClientId(string input) =>
        Task.FromResult(
            _list.Where(
                x =>
                    string.IsNullOrEmpty(input) || x.EnvClientId.ToLower().StartsWith(input)
            )
        );

    public Task<IEnumerable<Authorization>> GetAuthorizationsByEnvClientId(string input) =>
        throw new NotImplementedException();

    public async Task<IEnumerable<Authorization>> GetAuthorizationsByClientId(int clientId)
    {
        return _authorizations;
    }


    public Task<IEnumerable<UpdateDto>> GetUpdates(DateTime startDateTime, DateTime endDateTime) =>
        throw new NotImplementedException();

    public Task<IEnumerable<Client>> SearchByLastName(string input) =>
        Task.FromResult(
            _list.Where(
                x =>
                    string.IsNullOrEmpty(input) || x.LastName.ToLower().Contains(input)
            )
        );
}
