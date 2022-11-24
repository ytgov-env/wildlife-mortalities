using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Shared.Services;

public class ConservationOfficerService : IDisposable
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly AppDbContext _dbContext;

    public ConservationOfficerService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _dbContext = _dbContextFactory.CreateDbContext();
    }

    public void Dispose() => _dbContext.Dispose();

    public async Task<IReadOnlyList<ConservationOfficer>> GetConservationOfficers() =>
        await _dbContext.People.OfType<ConservationOfficer>().AsNoTracking().ToListAsync();

    public async Task<IEnumerable<ConservationOfficer>> SearchByLastName(string input) =>
        _dbContext.People
            .OfType<ConservationOfficer>()
            .Where(c => c.LastName.StartsWith(input));

    public async Task<IEnumerable<ConservationOfficer>> SearchByBadgeNumber(string input) =>
        _dbContext.People
            .OfType<ConservationOfficer>()
            .Where(c => c.BadgeNumber.StartsWith(input));
}
