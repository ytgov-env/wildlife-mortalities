using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reporters;

namespace WildlifeMortalities.Shared.Services;

public class MortalityService<T> where T : Mortality
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public MortalityService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<T?> GetMortalityById(int id)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();

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

    public async Task<IReadOnlyList<T>> GetMortalitiesByEnvClientId(string envClientId)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.Mortalities
            .Where(m => m.Reporter is Client && (m.Reporter as Client)!.EnvClientId == envClientId)
            .OfType<T>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetMortalitiesByConservationOfficerBadgeNumber(
        string conservationOfficerBadgeNumber
    )
    {
        var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.Mortalities
            .Where(
                m =>
                    m.Reporter is ConservationOfficer
                    && (m.Reporter as ConservationOfficer)!.BadgeNumber
                        == conservationOfficerBadgeNumber
            )
            .OfType<T>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T> CreateMortality(T mortality)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();
        var validationResult = new ValidationResult();
        //switch (mortality) {
        //    case BirdMortality bird:
        //        break;
        //    case BisonMortality bison:
        //        break;
        //    case BlackBearMortality blackbear:
        //        break;
        //    case CaribouMortality caribou:
        //        break;
        //    case CoyoteMortality coyote:
        //        break;
        //    case DeerMortality deer:
        //        break;
        //    case ElkMortality elk:
        //        break;
        //    case GoatMortality goat:
        //        break;
        //    case GrizzlyBearMortality grizzlybear:
        //        break;
        //    case MooseMortality moose:
        //        break;
        //    case SheepMortality sheep:
        //        break;
        //    case WolfMortality wolf:
        //        break;
        //    case WolverineMortality wolverine:
        //        break;
        //}

        context.Add(mortality);
        await context.SaveChangesAsync();
        return mortality;
    }

    public async Task<T> UpdateMortality(T mortality)
    {
        throw new NotImplementedException();
    }
}
