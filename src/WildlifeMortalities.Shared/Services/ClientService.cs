using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Shared.Services;

public class ClientService : IDisposable
{
    private readonly AppDbContext _dbContext;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public ClientService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _dbContext = _dbContextFactory.CreateDbContext();
    }

    public void Dispose() => _dbContext.Dispose();

    public async Task<IEnumerable<Client>> SearchByEnvClientId(string input) =>
        // await using var context = await _dbContextFactory.CreateDbContextAsync();
        _dbContext.People.OfType<Client>().Where(c => c.EnvClientId.StartsWith(input));

    public async Task<IEnumerable<Client>> SearchByLastName(string input) =>
        // await using var context = await _dbContextFactory.CreateDbContextAsync();
        _dbContext.People.OfType<Client>().Where(c => c.LastName.StartsWith(input));

    public async Task<IEnumerable<Authorization>> GetAuthorizationsByEnvClientId(string input) =>
        _dbContext.Authorizations.Where(a => a.Client.EnvClientId == input);

    public async Task<IEnumerable<Authorization>> GetAuthorizationsByClientId(int clientId) =>
        _dbContext.Authorizations.Where(a => a.Client.Id == clientId);
}
