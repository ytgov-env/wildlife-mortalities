using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Validators;

namespace WildlifeMortalities.Shared.Services;

public class MortalityService : IMortalityService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public MortalityService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<Mortality>> GetAllMortalities()
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.Mortalities.ToListAsync();
    }

    public async Task<T?> GetMortalityById<T>(int id) where T : Mortality
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var mortality = await context.Mortalities.FirstOrDefaultAsync(m => m.Id == id);
        if (mortality?.GetType() == typeof(T))
        {
            return mortality as T;
        }
        else
        {
            return null;
        }
    }

    public async Task<IReadOnlyList<T>> GetMortalitiesByEnvClientId<T>(string envClientId)
        where T : Mortality
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.Mortalities
            .OfType<T>()
            .Where(m => m.Reporter is Client && (m.Reporter as Client)!.EnvClientId == envClientId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetMortalitiesByConservationOfficerBadgeNumber<T>(
        string conservationOfficerBadgeNumber
    ) where T : Mortality
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.Mortalities
            .OfType<T>()
            .Where(
                m =>
                    m.Reporter is ConservationOfficer
                    && (m.Reporter as ConservationOfficer)!.BadgeNumber
                        == conservationOfficerBadgeNumber
            )
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result<T>> CreateMortality<T>(T mortality) where T : Mortality
    {
        var validator = new MortalityValidator<T>();
        var validation = await validator.ValidateAsync(mortality);
        if (!validation.IsValid)
        {
            return Result<T>.Invalid(validation.AsErrors());
        }
        return Result<T>.Success(mortality);
    }

    public async Task<Result<T>> UpdateMortality<T>(T mortality) where T : Mortality
    {
        var validator = new MortalityValidator<T>();
        var validation = await validator.ValidateAsync(mortality);
        if (!validation.IsValid)
        {
            return Result<T>.Invalid(validation.AsErrors());
        }
        return Result<T>.Success(mortality);
    }
}
