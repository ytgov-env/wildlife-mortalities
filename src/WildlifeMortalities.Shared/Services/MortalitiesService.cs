using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reporters;

namespace WildlifeMortalities.Shared.Services
{
    public class MortalitiesService<T> where T : Mortality
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public MortalitiesService(IDbContextFactory<AppDbContext> dbContextFactory)
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

        public async Task<List<T>> GetMortalitiesByEnvClientId(string envClientId)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();

            return await context.Mortalities.Where(m => m.Reporter is Client && (m.Reporter as Client)!.EnvClientId == envClientId).OfType<T>().ToListAsync();
        }

        public async Task<List<T>> GetMortalitiesByConservationOfficerBadgeNumber(string conservationOfficerBadgeNumber)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();

            return await context.Mortalities.Where(m => m.Reporter is ConservationOfficer && (m.Reporter as ConservationOfficer)!.BadgeNumber == conservationOfficerBadgeNumber).OfType<T>().ToListAsync();
        }

        public async Task<T> CreateMortality(T mortality)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
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

            var createdMortality = context.Add(mortality).Entity;
            await context.SaveChangesAsync();

            return createdMortality;
        }
    }
}
