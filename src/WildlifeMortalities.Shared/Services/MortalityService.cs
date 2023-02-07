using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Shared.Services;

public class MortalityService : IMortalityService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public MortalityService(IDbContextFactory<AppDbContext> dbContextFactory) =>
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

    public async Task CreateReport(IndividualHuntedMortalityReport report)
    {
        SetReportNavigationPropertyForMortalities(report);

        report.DateSubmitted = DateTimeOffset.Now;

        using var context = _dbContextFactory.CreateDbContext();
        context.Add(report);
        await context.SaveChangesAsync();
    }

    public async Task CreateReport(OutfitterGuidedHuntReport report)
    {
        SetReportNavigationPropertyForMortalities(report);

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
        report.DateSubmitted = DateTimeOffset.Now;

        do
        {
            report.GenerateHumanReadableId();
        } while (await context.Reports.AnyAsync(x => x.HumanReadableId == report.HumanReadableId));

        context.Add(report);
        await context.SaveChangesAsync();
    }

    public async Task CreateReport(SpecialGuidedHuntReport report)
    {
        SetReportNavigationPropertyForMortalities(report);

        report.DateSubmitted = DateTimeOffset.Now;
        using var context = _dbContextFactory.CreateDbContext();
        context.Add(report);
        await context.SaveChangesAsync();
    }

    public async Task CreateReport(HumanWildlifeConflictMortalityReport report)
    {
        SetReportNavigationPropertyForMortalities(report);

        using var context = _dbContextFactory.CreateDbContext();
        context.Add(report);
        await context.SaveChangesAsync();
    }

    public async Task CreateDraftReport(string report, int personId)
    {
        var draftReport = new DraftReport
        {
            LastModifiedDate = DateTimeOffset.Now,
            SerializedData = report,
            Type = report.GetType().Name,
            PersonId = personId,
        };

        using var context = _dbContextFactory.CreateDbContext();
        context.Add(draftReport);
        await context.SaveChangesAsync();
    }

    public async Task<ReportDetail?> GetReport(int id)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var result = await GetReportsIncludingMortalities(context)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (result == null)
        {
            return null;
        }

        var mortalities = result.GetMortalities();

        List<(int, BioSubmission)> bioSubmissions = new();
        foreach (var item in mortalities.OfType<IHasBioSubmission>())
        {
            BioSubmission? bioSubmission = null;
            bioSubmission = item switch
            {
                AmericanBlackBearMortality
                    => await context.BioSubmissions
                        .OfType<AmericanBlackBearBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                CanadaLynxMortality
                    => await context.BioSubmissions
                        .OfType<CanadaLynxBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                GreyWolfMortality
                    => await context.BioSubmissions
                        .OfType<GreyWolfBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                GrizzlyBearMortality
                    => await context.BioSubmissions
                        .OfType<GrizzlyBearBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                MountainGoatMortality
                    => await context.BioSubmissions
                        .OfType<MountainGoatBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                MuleDeerMortality
                    => await context.BioSubmissions
                        .OfType<MuleDeerBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                ThinhornSheepMortality
                    => await context.BioSubmissions
                        .OfType<ThinhornSheepBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                WhiteTailedDeerMortality
                    => await context.BioSubmissions
                        .OfType<WhiteTailedDeerBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                WoodBisonMortality
                    => await context.BioSubmissions
                        .OfType<WoodBisonBioSubmission>()
                        .FirstOrDefaultAsync(x => x.MortalityId == item.Id),
                _ => throw new InvalidOperationException()
            };

            if (bioSubmission != null)
            {
                bioSubmissions.Add((item.Id, bioSubmission));
            }
        }

        return new ReportDetail(result, bioSubmissions);
    }

    public async Task CreateBioSubmission(BioSubmission bioSubmission)
    {
        using var context = _dbContextFactory.CreateDbContext();

        context.BioSubmissions.Add(bioSubmission);
        await context.SaveChangesAsync();
    }

    public async Task UpdateBioSubmission(BioSubmission bioSubmission)
    {
        using var context = _dbContextFactory.CreateDbContext();

        context.BioSubmissions.Update(bioSubmission);
        await context.SaveChangesAsync();
    }

    // This method is required as the relationship fixup mechanism in EF Core does not handle this correctly
    private void SetReportNavigationPropertyForMortalities(Report report)
    {
        foreach (var item in report.GetMortalities())
        {
            item.Report = report;
        }
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

    private static IQueryable<Report> GetReportsIncludingMortalities(AppDbContext context) =>
        context.Reports
            .Include(x => ((SpecialGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Mortality)
            .Include(x => ((OutfitterGuidedHuntReport)x).HuntedActivities)
            .ThenInclude(x => x.Mortality)
            .Include(x => ((IndividualHuntedMortalityReport)x).HuntedActivity.Mortality);

    private static IQueryable<Report> GetReportsQuery(string? envClientId, AppDbContext context)
    {
        var query = GetReportsIncludingMortalities(context);

        if (string.IsNullOrEmpty(envClientId) == false)
        {
            query = query.Where(
                r =>
                    r is IndividualHuntedMortalityReport
                        ? ((IndividualHuntedMortalityReport)r).Client.EnvClientId == envClientId
                        : r is SpecialGuidedHuntReport
                            ? ((SpecialGuidedHuntReport)r).Client.EnvClientId == envClientId
                            : r is OutfitterGuidedHuntReport
                                ? ((OutfitterGuidedHuntReport)r).Client.EnvClientId == envClientId
                                : r is TrappedMortalitiesReport
                                    && ((TrappedMortalitiesReport)r).Client.EnvClientId
                                        == envClientId
            );
        }

        return query;
    }

    public async Task<IEnumerable<GameManagementArea>> GetGameManagementAreas()
    {
        using var context = _dbContextFactory.CreateDbContext();

        return await context.GameManagementAreas.ToArrayAsync();
    }

    public async Task<IEnumerable<OutfitterArea>> GetOutfitterAreas()
    {
        using var context = _dbContextFactory.CreateDbContext();

        return await context.OutfitterAreas
            .OrderBy(o => o.Area.Length)
            .ThenBy(o => o.Area)
            .ToArrayAsync();
    }
}
