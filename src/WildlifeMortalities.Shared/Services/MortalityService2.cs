using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Shared.Services;

public class MortalityService2 : IMortalityService2
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public MortalityService2(IDbContextFactory<AppDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public async Task<int> CountAllReports()
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await GetReportsQuery(null, context).CountAsync();
    }

    public async Task<int> CountReportsByEnvClientId(string envClientId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await GetReportsQuery(envClientId, context).CountAsync();
    }

    public async Task<IEnumerable<Report>> GetAllReports(int start = 0, int length = 10)
    {
        var result = await GetReports(null, start, length);

        return result;
    }

    public async Task<IEnumerable<Report>> GetReportsByEnvClientId(
        string envClientId,
        int start = 0,
        int length = 10
    )
    {
        var result = await GetReports(envClientId, start, length);

        return result;
    }

    public async Task CreateReport(HuntedMortalityReport report)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Add(report);
        await context.SaveChangesAsync();
    }

    public async Task CreateReport(OutfitterGuidedHuntReport report)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var guideIds = report.Guides.Select(x => x.Id).ToList();
        var guides = await context.People
            .OfType<Client>()
            .Where(x => guideIds.Contains(x.Id) == true)
            .ToListAsync();

        var area = await context.OutfitterAreas.FirstOrDefaultAsync(
            x => x.Area == report.OutfitterArea.Area
        );
        if (area != null)
        {
            report.OutfitterArea = area;
        }
        else
        {
            throw new ArgumentException(nameof(report.OutfitterArea));
        }

        report.Guides = guides;

        context.Add(report);
        await context.SaveChangesAsync();
    }

    public async Task CreateReport(SpecialGuidedHuntReport report)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Add(report);
        await context.SaveChangesAsync();
    }

    public async Task CreateReport(HumanWildlifeConflictMortalityReport report)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Add(report);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Report>> GetReports(
        string? envClientId,
        int start = 0,
        int length = 10
    )
    {
        using var context = _dbContextFactory.CreateDbContext();

        var query = GetReportsQuery(envClientId, context);

        var result = await query
            .OrderBy(x => x.DateSubmitted)
            .Skip(start)
            .Take(length)
            .AsSplitQuery()
            .ToArrayAsync();

        return result;
    }

    private static IQueryable<Report> GetReportsQuery(string? envClientId, AppDbContext context)
    {
        var query = context.Reports
            .Include(x => ((SpecialGuidedHuntReport)x).HuntedMortalityReports)
            .ThenInclude(x => x.Mortality)
            .Include(x => ((OutfitterGuidedHuntReport)x).HuntedMortalityReports)
            .ThenInclude(x => x.Mortality)
            .Include(x => ((MortalityReport)x).Mortality)
            .Where(
                x =>
                    x is HuntedMortalityReport
                        ? ((HuntedMortalityReport)x).OutfitterGuidedHuntReport == null
                          && ((HuntedMortalityReport)x).SpecialGuidedHuntReport == null
                        : true
            );

        if (string.IsNullOrEmpty(envClientId) == false)
        {
            query = query.Where(
                r =>
                    r is HuntedMortalityReport
                        ? ((HuntedMortalityReport)r).Client.EnvClientId == envClientId
                        : r is SpecialGuidedHuntReport
                            ? ((SpecialGuidedHuntReport)r).Client.EnvClientId == envClientId
                            : r is OutfitterGuidedHuntReport
                                ? ((OutfitterGuidedHuntReport)r).Client.EnvClientId
                                  == envClientId
                                : r is TrappedMortalityReport && ((TrappedMortalityReport)r).Client.EnvClientId
                                == envClientId
            );
        }

        return query;
    }
}
