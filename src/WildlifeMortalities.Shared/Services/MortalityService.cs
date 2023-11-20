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
using WildlifeMortalities.Shared.Extensions;
using WildlifeMortalities.Shared.Services.Reports.Single;
using WildlifeMortalities.Shared.Services.Rules;

namespace WildlifeMortalities.Shared.Services;

public class MortalityService : IMortalityService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public MortalityService(IDbContextFactory<AppDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public async Task<int> CreateReport(Report report, int userId, int? draftReportId = null)
    {
        SetReportNavigationPropertyForActivities(report, report);

        using var context = _dbContextFactory.CreateDbContext();

        if (draftReportId != null)
        {
            var draftReport =
                await context.DraftReports.FindAsync(draftReportId)
                ?? throw new ArgumentException(
                    $"Draft report {draftReportId} not found.",
                    nameof(draftReportId)
                );
            context.Remove(draftReport);
        }

        report.CreatedById =
            (await context.Users.FindAsync(userId))?.Id
            ?? throw new Exception($"User {userId} not found.");
        var now = DateTimeOffset.Now;
        report.DateCreated = now;

        foreach (var activity in report.GetActivities())
        {
            activity.CreatedTimestamp = now;
        }

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

        foreach (var activity in report.GetActivities())
        {
            activity.GenerateHumanReadableId();
            if (activity is HuntedActivity huntedActivity)
            {
                huntedActivity.GameManagementArea ??= await context.GameManagementAreas.FirstAsync(
                    x => x.Id == huntedActivity.GameManagementAreaId
                )!;
            }
        }

        context.Add(report);
        await AddDefaultBioSubmissions(context, report);

        await RulesSummary.GenerateAll(report, context);

        await context.SaveChangesAsync();
        return report.Id;
    }

    private static async Task CreateOrUpdateReport(
        AppDbContext context,
        IndividualHuntedMortalityReport report
    )
    {
        report.Season ??= await HuntingSeason.GetSeason(report, context);
        report.Person = await context.People
            .OfType<PersonWithAuthorizations>()
            .Include(x => x.Authorizations)
            .FirstAsync(x => x.Id == report.PersonId);
    }

    private static async Task CreateOrUpdateReport(
        AppDbContext context,
        OutfitterGuidedHuntReport report,
        OutfitterGuidedHuntReport? existingReport
    )
    {
        report.Client = await context.People
            .OfType<Client>()
            .FirstAsync(x => x.Id == report.ClientId);

        if (existingReport != null)
        {
            var existingChiefGuide = existingReport.ChiefGuide;
            if (
                existingChiefGuide != null
                && (
                    string.IsNullOrWhiteSpace(report.ChiefGuide!.FirstName)
                    || report.Result is Data.Enums.GuidedHuntResult.DidNotHunt
                )
            )
            {
                context.OutfitterGuides.Remove(existingChiefGuide);
            }
            else if (!string.IsNullOrWhiteSpace(report.ChiefGuide!.FirstName))
            {
                existingReport.ChiefGuide ??= new();
                existingReport.ChiefGuide.FirstName = report.ChiefGuide.FirstName;
                existingReport.ChiefGuide.LastName = report.ChiefGuide.LastName;
            }

            existingReport.AssistantGuides.AddRange(
                report.AssistantGuides.Where(
                    x => x.Id == 0 && !string.IsNullOrWhiteSpace(x.FirstName)
                )
            );
            foreach (var guide in report.AssistantGuides.Where(x => x.Id != 0))
            {
                var existingAssistantGuide = existingReport.AssistantGuides.Single(
                    x => x.Id == guide.Id
                );
                if (
                    string.IsNullOrWhiteSpace(guide.FirstName)
                    || report.Result is Data.Enums.GuidedHuntResult.DidNotHunt
                )
                {
                    context.OutfitterGuides.Remove(existingAssistantGuide);
                }
                else
                {
                    existingAssistantGuide.FirstName = guide.FirstName;
                    existingAssistantGuide.LastName = guide.LastName;
                }
            }
        }
        else
        {
            if (report.Result is Data.Enums.GuidedHuntResult.DidNotHunt)
            {
                report.ChiefGuide = null;
                report.AssistantGuides.Clear();
            }
            else
            {
                report.AssistantGuides.RemoveAll(x => string.IsNullOrWhiteSpace(x.FirstName));
            }
        }

        if (report.Result is Data.Enums.GuidedHuntResult.DidNotHunt)
        {
            report.HuntStartDate = null;
            report.HuntEndDate = null;
        }

        var area = await context.OutfitterAreas.FirstOrDefaultAsync(
            x => x.Area == report.OutfitterArea.Area
        );
        if (area != null)
        {
            report.OutfitterArea = area;
            report.OutfitterAreaId = area.Id;
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
        report.Client = await context.People
            .OfType<Client>()
            .FirstAsync(x => x.Id == report.ClientId);
    }

    private static async Task CreateOrUpdateReport(
        AppDbContext context,
        HumanWildlifeConflictMortalityReport report
    )
    {
        report.Season ??=
            await CalendarSeason.GetSeason(report, context)
            ?? throw new ArgumentException("Unable to resolve season.", nameof(report));
        report.ConservationOfficer = await context.People
            .OfType<ConservationOfficer>()
            .FirstAsync(x => x.Id == report.ConservationOfficerId);
    }

    private static async Task CreateOrUpdateReport(
        AppDbContext context,
        TrappedMortalitiesReport report
    )
    {
        report.Season ??= await TrappingSeason.GetSeason(report, context);
        report.Client = await context.People
            .OfType<Client>()
            .FirstAsync(x => x.Id == report.ClientId);

        var concession = await context.RegisteredTrappingConcessions.FirstOrDefaultAsync(
            x => x.Concession == report.RegisteredTrappingConcession.Concession
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

    public async Task UpdateReport(Report report, int userId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var existingReport = await context.Reports
            .WithEntireGraph()
            .FirstAsync(x => x.Id == report.Id);

        await RulesSummary.ResetAll(existingReport, context);

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

        report.PreserveImmutableValues(existingReport);
        report.LastModifiedById =
            (await context.Users.FindAsync(userId))?.Id
            ?? throw new Exception($"User {userId} not found.");
        var now = DateTimeOffset.Now;
        report.DateModified = now;

        context.Entry(existingReport).CurrentValues.SetValues(report);

        await UpdateActivities(context, report, existingReport, now);
        await AddDefaultBioSubmissions(context, report);

        await RulesSummary.GenerateAll(report, context);

        await context.SaveChangesAsync();

        static async Task UpdateActivities(
            AppDbContext context,
            Report report,
            Report existingReport,
            DateTimeOffset now
        )
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

            Dictionary<Activity, Activity> replacements = new();
            foreach (var activity in report.GetActivities())
            {
                await SetArea(activity, context);

                if (activity.Id > 0)
                {
                    var existingActivity = existingReport
                        .GetActivities()
                        .First(x => x.Id == activity.Id);

                    activity.PreserveImmutableValues(existingActivity);

                    context.Entry(existingActivity).CurrentValues.SetValues(activity);
                    if (existingActivity is HarvestActivity existingHarvestActivity)
                    {
                        existingHarvestActivity.Authorizations.Clear();
                    }
                    activityIdsToDelete.Remove(activity.Id);
                    replacements.Add(activity, existingActivity);
                }
                else
                {
                    activity.CreatedTimestamp = now;
                    activity.GenerateHumanReadableId();
                    context.Add(activity);
                }
            }

            foreach (var activityId in activityIdsToDelete)
            {
                var existingActivity = await context.Activities.FindAsync(activityId);
                if (existingActivity is not null)
                {
                    context.Activities.Remove(existingActivity);
                }
            }

            report.OverrideActivity(replacements);
        }
    }

    private static async Task SetArea(Activity activity, AppDbContext context)
    {
        switch (activity)
        {
            case HuntedActivity huntedActivity:
                huntedActivity.GameManagementArea = await context.GameManagementAreas.FirstAsync(
                    x => x.Id == huntedActivity.GameManagementAreaId
                );
                break;
            default:
                throw new System.Diagnostics.UnreachableException();
        }
    }

    public async Task<int> CreateDraftReport(
        string reportType,
        string report,
        int personId,
        int userId
    )
    {
        using var context = _dbContextFactory.CreateDbContext();

        var createdById =
            (await context.Users.FindAsync(userId))?.Id
            ?? throw new Exception($"User {userId} not found.");
        var draftReport = new DraftReport
        {
            Type = reportType,
            PersonId = personId,
            DateCreated = DateTimeOffset.Now,
            CreatedById = createdById,
            SerializedData = report,
        };

        context.Add(draftReport);
        await context.SaveChangesAsync();
        return draftReport.Id;
    }

    public async Task UpdateDraftReport(string report, int reportId, int userId)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var reportFromDatabase =
            await context.DraftReports.FindAsync(reportId)
            ?? throw new ArgumentException($"Draft report {reportId} not found.", nameof(reportId));
        reportFromDatabase.SerializedData = report;
        reportFromDatabase.LastModifiedById =
            (await context.Users.FindAsync(userId))?.Id
            ?? throw new Exception($"User {userId} not found.");
        reportFromDatabase.DateLastModified = DateTimeOffset.Now;

        await context.SaveChangesAsync();
    }

    public async Task SoftDeleteReport(string report, int reportId, int userId, string reason)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var existingReport = await context.Reports
            .WithEntireGraph()
            .FirstAsync(x => x.Id == reportId);

        var deletedReport = new DeletedReport
        {
            PersonId = existingReport.GetPerson().Id,
            HumanReadableId = existingReport.HumanReadableId,
            Season = existingReport.Season,
            DateLastModified = existingReport.DateModified,
            DateSubmitted = existingReport.DateSubmitted,
            DateDeleted = DateTimeOffset.Now,
            Reason = reason,
            DeletedById = userId,
            SerializedData = report,
        };

        context.Add(deletedReport);

        await RulesSummary.ResetAll(existingReport, context);
        context.Remove(existingReport);

        await context.SaveChangesAsync();
    }

    private static async Task AddDefaultBioSubmissions(AppDbContext context, Report report)
    {
        var reportDetails =
            report.Id > 0 ? await context.Reports.GetDetails(report.Id, context) : null;

        reportDetails ??= new ReportDetail(null!, Array.Empty<(int, BioSubmission)>());

        foreach (var mortality in report.GetMortalities().OfType<IHasBioSubmission>())
        {
            var existingBioSubmission = reportDetails.BioSubmissions.FirstOrDefault(
                x => x.mortalityId == mortality.Id
            );

            if (!mortality.SubTypeHasBioSubmission() && existingBioSubmission != default)
            {
                context.BioSubmissions.Remove(existingBioSubmission.submission);
            }

            if (existingBioSubmission == default)
            {
                var bioSubmission = mortality.CreateDefaultBioSubmission();
                if (bioSubmission == null)
                {
                    continue;
                }

                context.BioSubmissions.Add(bioSubmission);
            }
        }
    }

    private async Task<ReportDetail> UpdateBioSubmission(
        BioSubmission bioSubmission,
        int reportId,
        int userId,
        Action updater
    )
    {
        var context = _dbContextFactory.CreateDbContext();

        var biosubmissionId = bioSubmission.Id;
        bioSubmission.Id = 0;
        var submissionFromDb = await context.BioSubmissions.FirstOrDefaultAsync(
            x => x.Id == biosubmissionId
        );
        var strategy = context.Database.CreateExecutionStrategy();
        var report = await context.Reports.WithEntireGraph().SingleAsync(x => x.Id == reportId);

        await strategy.Execute(async () =>
        {
            using var transaction = context.Database.BeginTransaction();
            await RulesSummary.ResetAll(report, context);
            updater();

            bioSubmission.LastModifiedBy =
                (await context.Users.FindAsync(userId))
                ?? throw new Exception($"User {userId} not found.");

            bioSubmission.DateModified = DateTimeOffset.Now;

            if (submissionFromDb != null)
            {
                context.BioSubmissions.Remove(submissionFromDb);
                await context.SaveChangesAsync();
            }

            bioSubmission.ClearDependencies();
            context.BioSubmissions.Add(bioSubmission);

            await RulesSummary.GenerateAll(report, context);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        });

        return await report.GetDetails(context);
    }

    public async Task<ReportDetail> UpdateOrganicMaterialForBioSubmission(
        BioSubmission bioSubmission,
        int reportId,
        int userId
    ) =>
        await UpdateBioSubmission(
            bioSubmission,
            reportId,
            userId,
            () =>
            {
                bioSubmission.UpdateRequiredOrganicMaterialsStatus();
                if (
                    bioSubmission.RequiredOrganicMaterialsStatus
                    != BioSubmissionRequiredOrganicMaterialsStatus.Submitted
                )
                {
                    bioSubmission.DateSubmitted = null;
                }
            }
        );

    public async Task<ReportDetail> UpdateBioSubmissionAnalysis(
        BioSubmission bioSubmission,
        int reportId,
        int userId
    ) =>
        await UpdateBioSubmission(
            bioSubmission,
            reportId,
            userId,
            bioSubmission.UpdateAnalysisStatus
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
            .OrderBy(o => o.Concession.Length)
            .ThenBy(o => o.Concession)
            .ToArrayAsync();
    }

    // This method is required as the relationship fixup mechanism in EF Core does not handle this correctly
    private static void SetReportNavigationPropertyForActivities(
        Report sourceReport,
        Report destinationReport
    )
    {
        foreach (var activity in sourceReport.GetActivities())
        {
            switch (activity)
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
