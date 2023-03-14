using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Shared.Services;

public class ClientService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public ClientService(IDbContextFactory<AppDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public async Task<IEnumerable<Client>> SearchByEnvClientId(string input)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.People
            .OfType<Client>()
            .Where(c => c.EnvClientId.StartsWith(input))
            .ToArrayAsync();
    }

    public async Task<int> GetPersonIdByEnvClientId(string envClientID)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.People
            .OfType<Client>()
            .Where(c => c.EnvClientId == envClientID)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<Client?> GetClientByEnvClientId(string envClientID)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.People
            .OfType<Client>()
            .Where(c => c.EnvClientId == envClientID)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Client>> SearchByLastName(string input)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.People
            .OfType<Client>()
            .Where(c => c.LastName.StartsWith(input))
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Authorization>> GetAuthorizationsByEnvClientId(string input)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Authorizations
            .Where(a => a.Client.EnvClientId == input)
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Authorization>> GetAuthorizationsByClientId(int clientId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Authorizations.Where(a => a.Client.Id == clientId).ToArrayAsync();
    }
}
