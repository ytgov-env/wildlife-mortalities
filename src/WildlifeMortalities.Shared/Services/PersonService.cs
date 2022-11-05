﻿using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public class PersonService<T> where T : Person
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public PersonService(IDbContextFactory<AppDbContext> dbContextFactory) => _dbContextFactory = dbContextFactory;

    public async Task<IReadOnlyList<ConservationOfficer>> GetConservationOfficers()
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.People.OfType<ConservationOfficer>().AsNoTracking().ToListAsync();
    }

    public async Task<ConservationOfficer?> GetConservationOfficerById(int id)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.People
            .OfType<ConservationOfficer>()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<ConservationOfficer?> GetConservationOfficerByBadgeNumber(string badgeNumber)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.People
            .OfType<ConservationOfficer>()
            .FirstOrDefaultAsync(c => c.BadgeNumber == badgeNumber);
    }

    public async Task<IReadOnlyList<Client>> GetClients() => throw new NotImplementedException();

    public async Task<Client> GetClientById(int id) => throw new NotImplementedException();

    public async Task<Client> GetClientByEnvClientId(string envClientId) => throw new NotImplementedException();
}
