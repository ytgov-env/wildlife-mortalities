using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Shared.Services;
public class HuntedHarvestReportService<T> where T : Mortality
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly MortalityService<T> _mortalityService;

    public HuntedHarvestReportService(IDbContextFactory<AppDbContext> dbContextFactory, MortalityService<T> mortalityService)
    {
        _dbContextFactory = dbContextFactory;
        _mortalityService = mortalityService;
    }

    public async Task<HuntedHarvestReport?> GetHarvestReportById(int id)
    {
        var context = await _dbContextFactory.CreateDbContextAsync();

        return await context.HarvestReports.OfType<HuntedHarvestReport>().FirstOrDefaultAsync(h => h.Id == id);
    }
}
