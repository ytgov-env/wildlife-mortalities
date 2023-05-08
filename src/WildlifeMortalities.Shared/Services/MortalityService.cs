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
using WildlifeMortalities.Shared.Services.Reports.Single;

namespace WildlifeMortalities.Shared.Services;

public class MortalityService : IMortalityService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public MortalityService(IDbContextFactory<AppDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public async Task CreateReport(Report report)
    {
        SetReportNavigationPropertyForActivities(report, report);
        report.DateSubmitted = DateTimeOffset.Now;
        report.DateModified = DateTimeOffset.Now;

        using var context = _dbContextFactory.CreateDbContext();
        switch (report)
        {
            case IndividualHuntedMortalityReport individualHuntedMortalityReport:
                await CreateOrUpdateReport(context, individualHuntedMortalityReport);
                break;
            case SpecialGuidedHuntReport specialGuidedHuntReport:
                await CreateOrUpdateReport(context, specialGuidedHuntReport, null);
                break;
            case OutfitterGuidedHuntReport outfitterGuidedHuntReport:
                await CreateOrUpdateReport(context, outfitterGuidedHuntReport, null);
                break;
            case HumanWildlifeConflictMortalityReport humanWildlifeConflictMortalityReport:
                await CreateOrUpdateReport(context, humanWildlifeConflictMortalityReport);
                break;
            case TrappedMortalitiesReport trappedMortalitiesReport:
                await CreateOrUpdateReport(context, trappedMortalitiesReport);
                break;
            default:
                throw new NotImplementedException();
        }

        do
        {
            report.GenerateHumanReadableId();
        } while (await context.Reports.AnyAsync(x => x.HumanReadableId == report.HumanReadableId));

        context.Add(report);
        await AddDefaultBioSubmissions(context, report);
        await context.SaveChangesAsync();
    }

    private static async Task CreateOrUpdateReport(
        AppDbContext context,
        IndividualHuntedMortalityReport report
    )
    {
        report.Season ??= await HuntingSeason.GetSeason(report, context);
    }

    private static async Task CreateOrUpdateReport(
        AppDbContext context,
        OutfitterGuidedHuntReport report,
        OutfitterGuidedHuntReport? existingReport
    )
    {
        var newAssistantGuideIds = report.AssistantGuides.ConvertAll(x => x.Id);

        var assistantGuides = await context.People
            .OfType<Client>()
            .Where(x => newAssistantGuideIds.Contains(x.Id))
            .ToListAsync();

        if (existingReport != null)
        {
            var existingAssistantGuideIds =
                existingReport.AssistantGuides.Select(y => y.Id) ?? new List<int>();

            var idsToAdd = newAssistantGuideIds
                .Except(newAssistantGuideIds.Intersect(existingAssistantGuideIds))
                .ToArray();

            existingReport.AssistantGuides.AddRange(
                assistantGuides.Where(x => idsToAdd.Contains(x.Id)).ToArray()
            );

            var idsToRemove = existingAssistantGuideIds
                .Except(existingAssistantGuideIds.Intersect(newAssistantGuideIds))
                .ToArray();

            foreach (var id in idsToRemove)
            {
                var existingGuide = existingReport.AssistantGuides.First(x => x.Id == id);
                existingReport.AssistantGuides.Remove(existingGuide);
            }
        }
        else
        {
            report.AssistantGuides = assistantGuides;
        }

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

        report.Season ??= await HuntingSeason.GetSeason(report, context);
    }

    private static async Task CreateOrUpdateReport(
        AppDbContext context,
        SpecialGuidedHuntReport report,
        SpecialGuidedHuntReport? existingReport
    )
    {
        report.Season ??= await HuntingSeason.GetSeason(report, context);
    }

    private static async Task CreateOrUpdateReport(
        AppDbContext context,
        HumanWildlifeConflictMortalityReport report
    )
    {
        report.Season ??=
            await CalendarSeason.GetSeason(report, context)
            ?? throw new ArgumentException("Unable to resolve season.", nameof(report));
    }

    private static async Task CreateOrUpdateReport(
        AppDbContext context,
        TrappedMortalitiesReport report
    )
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

    public async Task UpdateReport(Report report)
    {
        static void UpdateActivities(AppDbContext context, Report report, Report existingReport)
        {
            foreach (var mortality in report.GetMortalities())
            {
                if (mortality.Id > 0)
                {
                    var existingMortality = existingReport
                        .GetMortalities()
                        .First(x => x.Id == mortality.Id);

                    mortality.ActivityId = existingMortality.ActivityId;
                    if (mortality.Species != existingMortality.Species)
                    {
                        context.Mortalities.Remove(existingMortality);
                        mortality.Id = 0;
                        context.Add(mortality);

                        if (
                            mortality is IHasBioSubmission bioSubmissionMortality
                            && bioSubmissionMortality.CreateDefaultBioSubmission()
                                is BioSubmission bioSubmission
                            && bioSubmission is not null
                        )
                        {
                            context.BioSubmissions.Add(bioSubmission);
                        }
                    }
                    else
                    {
                        context.Entry(existingMortality).CurrentValues.SetValues(mortality);
                    }
                }
                else
                {
                    context.Add(mortality);
                }
            }

            var activityIdsToDelete = existingReport.GetActivities().Select(x => x.Id).ToList();

            foreach (var activity in report.GetActivities())
            {
                if (activity.Id > 0)
                {
                    var existingActivity = existingReport
                        .GetActivities()
                        .First(x => x.Id == activity.Id);

                    context.Entry(existingActivity).CurrentValues.SetValues(activity);

                    activityIdsToDelete.Remove(activity.Id);
                }
                else
                {
                    context.Add(activity);
                }
            }

            foreach (var activityId in activityIdsToDelete)
            {
                var existingActivity = existingReport
                    .GetActivities()
                    .First(x => x.Id == activityId);
                context.Activities.Remove(existingActivity);
            }
        }

        using var context = _dbContextFactory.CreateDbContext();
        var existingReport = await context.Reports
            .WithEntireGraph()
            .FirstAsync(x => x.Id == report.Id);

        SetReportNavigationPropertyForActivities(report, existingReport);

        switch (report)
        {
            case IndividualHuntedMortalityReport individualHuntedMortalityReport:
                await CreateOrUpdateReport(context, individualHuntedMortalityReport);
                break;
            case SpecialGuidedHuntReport specialGuidedHuntReport:
                await CreateOrUpdateReport(
                    context,
                    specialGuidedHuntReport,
                    (SpecialGuidedHuntReport)existingReport
                );
                break;
            case OutfitterGuidedHuntReport outfitterGuidedHuntReport:
                await CreateOrUpdateReport(
                    context,
                    outfitterGuidedHuntReport,
                    (OutfitterGuidedHuntReport)existingReport
                );
                break;
            case HumanWildlifeConflictMortalityReport humanWildlifeConflictMortalityReport:
                await CreateOrUpdateReport(context, humanWildlifeConflictMortalityReport);
                break;
            case TrappedMortalitiesReport trappedMortalitiesReport:
                await CreateOrUpdateReport(context, trappedMortalitiesReport);
                break;
            default:
                throw new NotImplementedException();
        }

        report.Discriminator = existingReport.Discriminator;
        report.HumanReadableId = existingReport.HumanReadableId;
        report.DateModified = DateTimeOffset.Now;

        context.Entry(existingReport).CurrentValues.SetValues(report);

        await AddDefaultBioSubmissions(context, report);
        UpdateActivities(context, report, existingReport);

        await context.SaveChangesAsync();
    }

    public async Task CreateDraftReport(string reportType, string report, int personId)
    {
        var now = DateTimeOffset.Now;
        var draftReport = new DraftReport
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

        var reportFromDatabase =
            await context.DraftReports.FindAsync(reportId)
            ?? throw new ArgumentException($"Draft report {reportId} not found.", nameof(reportId));
        reportFromDatabase.SerializedData = report;
        reportFromDatabase.DateLastModified = DateTimeOffset.Now;

        await context.SaveChangesAsync();
    }

    private static async Task AddDefaultBioSubmissions(AppDbContext context, Report report)
    {
        var reportDetails =
            report.Id > 0 ? await context.Reports.GetDetails(report.Id, context) : null;

        reportDetails ??= new ReportDetail(null!, Array.Empty<(int, BioSubmission)>());

        foreach (var item in report.GetMortalities().OfType<IHasBioSubmission>())
        {
            if (!reportDetails.BioSubmissions.Any(x => x.mortalityId == item.Id))
            {
                var bioSubmission = item.CreateDefaultBioSubmission();
                if (bioSubmission == null)
                {
                    continue;
                }
                context.BioSubmissions.Add(bioSubmission);
            }
        }
    }

    private async Task UpdateBioSubmission(BioSubmission bioSubmission, Action updater)
    {
        updater();
        bioSubmission.DateModified = DateTimeOffset.Now;

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
                var previousStatus = bioSubmission.RequiredOrganicMaterialsStatus;
                bioSubmission.UpdateRequiredOrganicMaterialsStatus();
                if (
                    previousStatus != BioSubmissionRequiredOrganicMaterialsStatus.Submitted
                    && bioSubmission.RequiredOrganicMaterialsStatus
                        == BioSubmissionRequiredOrganicMaterialsStatus.Submitted
                )
                {
                    bioSubmission.DateSubmitted = DateTimeOffset.Now;
                }

                //Todo: raise violation for other statuses

                bioSubmission.DateModified = DateTimeOffset.Now;
            }
        );

    public async Task UpdateBioSubmissionAnalysis(BioSubmission bioSubmission) =>
        await UpdateBioSubmission(bioSubmission, bioSubmission.UpdateAnalysisStatus);

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
    private static void SetReportNavigationPropertyForActivities(
        Report sourceReport,
        Report destinationReport
    )
    {
        foreach (var item in sourceReport.GetActivities())
        {
            switch (item)
            {
                case TrappedActivity trappedActivity:
                    trappedActivity.TrappedMortalitiesReport =
                        (TrappedMortalitiesReport)destinationReport;
                    break;
                case HumanWildlifeConflictActivity humanWildlifeConflictActivity:
                    humanWildlifeConflictActivity.Report =
                        (HumanWildlifeConflictMortalityReport)destinationReport;
                    break;
                case ResearchActivity researchActivity:
                    researchActivity.Report = (ResearchMortalityReport)destinationReport;
                    break;
                case CollaredActivity collaredActivity:
                    collaredActivity.Report = (CollaredMortalityReport)destinationReport;
                    break;
                case HuntedActivity huntedActivity:
                    switch (destinationReport)
                    {
                        case IndividualHuntedMortalityReport individualHuntedMortalityReport:
                            huntedActivity.IndividualHuntedMortalityReport =
                                individualHuntedMortalityReport;
                            break;
                        case OutfitterGuidedHuntReport outfitterGuidedHuntReport:
                            huntedActivity.OutfitterGuidedHuntReport = outfitterGuidedHuntReport;
                            break;
                        case SpecialGuidedHuntReport specialGuidedHuntReport:
                            huntedActivity.SpecialGuidedHuntReport = specialGuidedHuntReport;
                            break;
                    }
                    break;
            }
        }
    }
}
