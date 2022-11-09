using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public class ClientService : IDisposable
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private AppDbContext _dbContext;

    public ClientService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _dbContext = _dbContextFactory.CreateDbContext();
    }

    public async Task<IEnumerable<Client>> SearchByEnvClientId(string input)
    {
        // await using var context = await _dbContextFactory.CreateDbContextAsync();

        return _dbContext.People.OfType<Client>().Where(c => c.EnvClientId.StartsWith(input));
    }

    public async Task<IEnumerable<Client>> SearchByLastName(string input)
    {
        // await using var context = await _dbContextFactory.CreateDbContextAsync();

        return _dbContext.People.OfType<Client>().Where(c => c.LastName.StartsWith(input));
    }

    public async Task<IEnumerable<Authorization>> GetAuthorizationsByEnvClientId(string input)
    {
        return _dbContext.Authorizations.Where(a => a.Client.EnvClientId == input);
    }

    public async Task<IEnumerable<Authorization>> GetAuthorizationsByClientId(int clientId)
    {
        return _dbContext.Authorizations.Where(a => a.Client.Id == clientId);
    }

    public async Task<IEnumerable<UpdateDto>> GetUpdates(DateTime startDateTime, DateTime endDateTime) => throw new NotImplementedException();



    public void Dispose() => _dbContext.Dispose();
}
