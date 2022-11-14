using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Shared.Services;

public class ConservationOfficerService : IDisposable
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private AppDbContext _dbContext;

    public ConservationOfficerService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _dbContext = _dbContextFactory.CreateDbContext();
    }

    public async Task<IReadOnlyList<ConservationOfficer>> GetConservationOfficers()
    {
        return await _dbContext.People.OfType<ConservationOfficer>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<ConservationOfficer>> SearchByLastName(string input)
    {
        return _dbContext.People
            .OfType<ConservationOfficer>()
            .Where(c => c.LastName.StartsWith(input));
    }

    public async Task<IEnumerable<ConservationOfficer>> SearchByBadgeNumber(string input)
    {
        return _dbContext.People
            .OfType<ConservationOfficer>()
            .Where(c => c.BadgeNumber.StartsWith(input));
    }

    public void Dispose() => _dbContext.Dispose();
}
