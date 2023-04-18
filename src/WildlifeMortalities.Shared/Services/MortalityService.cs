using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Seasons;

namespace WildlifeMortalities.Shared.Services;

public class MortalityService : IMortalityService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public MortalityService(IDbContextFactory<AppDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public async Task CreateReport(Report report)
    {
        SetReportNavigationPropertyForMortalities(report);
        report.DateSubmitted = DateTimeOffset.Now;

        using var context = _dbContextFactory.CreateDbContext();
        switch (report)
        {
            case IndividualHuntedMortalityReport individualHuntedMortalityReport:
                await CreateReport(context, individualHuntedMortalityReport);
                break;
            case SpecialGuidedHuntReport specialGuidedHuntReport:
                await CreateReport(context, specialGuidedHuntReport);
                break;
            case OutfitterGuidedHuntReport outfitterGuidedHuntReport:
                await CreateReport(context, outfitterGuidedHuntReport);
                break;
            case HumanWildlifeConflictMortalityReport humanWildlifeConflictMortalityReport:
                await CreateReport(context, humanWildlifeConflictMortalityReport);
                break;
            case TrappedMortalitiesReport trappedMortalitiesReport:
                await CreateReport(context, trappedMortalitiesReport);
                break;
            default:
                throw new NotImplementedException();
        }

        do
        {
            report.GenerateHumanReadableId();
        } while (await context.Reports.AnyAsync(x => x.HumanReadableId == report.HumanReadableId));

        context.Add(report);
        AddDefaultBioSubmissions(context, report);
        await context.SaveChangesAsync();
    }

    private static async Task CreateReport(
        AppDbContext context,
        IndividualHuntedMortalityReport report
    )
    {
        report.Season ??= await HuntingSeason.GetSeason(report, context);
    }

    private static async Task CreateReport(AppDbContext context, OutfitterGuidedHuntReport report)
    {
        var assistantGuideIds = report.AssistantGuides.Select(x => x.Id).ToList();
        var assistantGuides = await context.People
            .OfType<Client>()
            .Where(x => assistantGuideIds.Contains(x.Id) == true)
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

        report.AssistantGuides = assistantGuides;

        report.Season ??= await HuntingSeason.GetSeason(report, context);
    }

    private static async Task CreateReport(AppDbContext context, SpecialGuidedHuntReport report)
    {
        report.Season ??= await HuntingSeason.GetSeason(report, context);
    }

    private static async Task CreateReport(
        AppDbContext context,
        HumanWildlifeConflictMortalityReport report
    )
    {
        report.Season ??=
            await CalendarSeason.GetSeason(report, context)
            ?? throw new ArgumentException("Unable to resolve season.", nameof(report));
    }

    private static async Task CreateReport(AppDbContext context, TrappedMortalitiesReport report)
    {
        report.Season ??= await TrappingSeason.GetSeason(report, context);

        var concession = await context.RegisteredTrappingConcessions.FirstOrDefaultAsync(
            x => x.Area == report.RegisteredTrappingConcession.Area
        );
        if (concession != null)
        {
            report.RegisteredTrappingConcession = concession;
        }
        else
        {
            throw new ArgumentException(nameof(report.RegisteredTrappingConcession));
        }
    }

    private static void AddDefaultBioSubmissions(AppDbContext context, Report report)
    {
        foreach (var item in report.GetMortalities().OfType<IHasBioSubmission>())
        {
            var bioSubmission = item.CreateDefaultBioSubmission();
            context.BioSubmissions.Add(bioSubmission);
        }
    }

    public async Task CreateDraftReport(string reportType, string report, int personId)
    {
        var now = DateTimeOffset.Now;
        DraftReport? draftReport = new DraftReport
        {
            DateLastModified = now,
            DateSubmitted = now,
            SerializedData = report,
            Type = reportType,
            PersonId = personId
        };

        using var context = _dbContextFactory.CreateDbContext();
        context.Add(draftReport);
        await context.SaveChangesAsync();
    }

    public async Task UpdateDraftReport(string report, int reportId)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var reportFromDatabase = await context.DraftReports.FindAsync(reportId);

        if (reportFromDatabase == null)
        {
            throw new ArgumentException($"Draft report {reportId} not found.", nameof(reportId));
        }

        reportFromDatabase.SerializedData = report;
        reportFromDatabase.DateLastModified = DateTimeOffset.Now;

        await context.SaveChangesAsync();
    }

    public async Task UpdateBioSubmissionAnalysis(BioSubmission bioSubmission) =>
        await UpdateBioSubmission(
            bioSubmission,
            () =>
            {
                var biosubmissionId = bioSubmission.Id;
                bioSubmission.Id = 0;

                if (bioSubmission is IHasHornMeasurementEntries submission)
                {
                    if (submission.HornMeasured.HasValue)
                    {
                        submission.BroomedStatus = null;
                        submission.HornTipSpreadMillimetres = null;
                        submission.HornTotalLengthMillimetres = null;
                        submission.HornBaseCircumferenceMillimetres = null;
                        submission.HornMeasurementEntries.Clear();

                        if (submission is ThinhornSheepBioSubmission sub)
                        {
                            sub.PlugNumber = string.Empty;
                            sub.HornLengthToThirdAnnulusMillimetres = null;
                        }
                    }

                    submission.HornMeasurementEntries.RemoveAll(x => x.IsBroomed);
                }
            }
        );

    private async Task UpdateBioSubmission(BioSubmission bioSubmission, Action updater)
    {
        updater();

        var context = _dbContextFactory.CreateDbContext();

        var biosubmissionId = bioSubmission.Id;
        bioSubmission.Id = 0;
        var submissionFromDb = await context.BioSubmissions.FirstOrDefaultAsync(
            x => x.Id == biosubmissionId
        );
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.Execute(async () =>
        {
            using var transaction = context.Database.BeginTransaction();

            if (submissionFromDb != null)
            {
                context.BioSubmissions.Remove(submissionFromDb);
                await context.SaveChangesAsync();
            }

            bioSubmission.ClearDependencies();
            context.BioSubmissions.Add(bioSubmission);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        });
    }

    public async Task UpdateOrganicMaterialForBioSubmission(BioSubmission bioSubmission) =>
        await UpdateBioSubmission(
            bioSubmission,
            () =>
            {
                var isSubmitted = bioSubmission.HasSubmittedAllRequiredOrganicMaterial();

                if (isSubmitted)
                {
                    bioSubmission.Status = BioSubmissionStatus.Submitted;
                    bioSubmission.DateSubmitted ??= DateTimeOffset.Now;
                }
                else
                {
                    bioSubmission.Status = BioSubmissionStatus.PartiallySubmitted;
                    //Todo: raise violation
                }
                bioSubmission.DateModified = DateTimeOffset.Now;
            }
        );

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

    public async Task<IEnumerable<RegisteredTrappingConcession>> GetRegisteredTrappingConcessions()
    {
        using var context = _dbContextFactory.CreateDbContext();

        return await context.RegisteredTrappingConcessions
            .OrderBy(o => o.Area.Length)
            .ThenBy(o => o.Area)
            .ToArrayAsync();
    }

    // This method is required as the relationship fixup mechanism in EF Core does not handle this correctly
    private static void SetReportNavigationPropertyForMortalities(Report report)
    {
        foreach (var item in report.GetMortalities())
        {
            item.Report = report;
        }
    }
}
