using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public class ReporterService<T> where T : Reporter
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public ReporterService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyList<ConservationOfficer>> GetConservationOfficers()
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.Reporters.OfType<ConservationOfficer>().AsNoTracking().ToListAsync();
    }

    public async Task<ConservationOfficer?> GetConservationOfficerById(int id)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.Reporters.OfType<ConservationOfficer>().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<ConservationOfficer?> GetConservationOfficerByBadgeNumber(string badgeNumber)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.Reporters.OfType<ConservationOfficer>().FirstOrDefaultAsync(c => c.BadgeNumber == badgeNumber);
    }

    public async Task<IReadOnlyList<ClientDto>> GetClients()
    {
        throw new NotImplementedException();
    }

    public async Task<ClientDto> GetClientById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ClientDto> GetClientByEnvClientId(string envClientId)
    {
        throw new NotImplementedException();
    }
}
