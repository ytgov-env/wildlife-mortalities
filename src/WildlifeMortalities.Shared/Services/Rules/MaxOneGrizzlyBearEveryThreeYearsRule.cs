using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services.Reports.Single;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.Shared.Services.Rules;

internal class MaxOneGrizzlyBearEveryThreeYearsRule : Rule
{
    public static void Reset((Report report, Activity activity)[] activitiesInViolation)
    {
        foreach (var (_, activity) in activitiesInViolation)
        {
            activity.Violations.RemoveAll(
                x => x.Rule == Violation.RuleType.KilledMoreThanOneGrizzlyBearInThreeYearSpan
            );
        }
    }

    public override async Task Reset(Report report, AppDbContext context)
    {
        var activitiesInViolation = await GetRelevantReportsWithGrizzluActivites(report, context);
        Reset(activitiesInViolation);
    }

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (report.GeneralizedReportType is not GeneralizedReportType.Hunted)
        {
            return RuleResult.NotApplicable;
        }

        var violations = new List<Violation>();
        var isApplicable = false;
        var activitiesInViolation = await GetRelevantReportsWithGrizzluActivites(report, context);

        foreach (var (report2, activity) in activitiesInViolation.Skip(1))
        {
            var violation = new Violation(
                activity,
                Violation.RuleType.KilledMoreThanOneGrizzlyBearInThreeYearSpan,
                Violation.SeverityType.Illegal,
                "Already killed a grizzly bear within the last three years."
            );
            activity.Violations.Add(violation);

            if (report == report2)
            {
                isApplicable = true;
            }
            else
            {
                //context.Violations.Add(violation);
            }
        }

        return !isApplicable
            ? RuleResult.NotApplicable
            : violations.Any()
                ? RuleResult.IsIllegal(violations)
                : RuleResult.IsLegal;
    }

    private static async Task<(
        Report report,
        Activity activity
    )[]> GetRelevantReportsWithGrizzluActivites(Report report, AppDbContext context)
    {
        var personId = report.GetPerson().Id;
        var currentSeason = (HuntingSeason)report.Season;
        var seasons = await context.Seasons
            .OfType<HuntingSeason>()
            .OrderBy(x => x.StartDate)
            .Select(x => x.Id)
            .ToListAsync();
        var index = seasons.IndexOf(currentSeason.Id);
        var applicableSeasonIds = new[] { seasons[index - 2], seasons[index - 1], seasons[index] };

        var reports = await context.Reports
            .WithEntireGraph()
            .Where(
                x =>
                    (
                        (
                            x is IndividualHuntedMortalityReport
                            && ((IndividualHuntedMortalityReport)x).PersonId == personId
                        )
                        || (
                            x is SpecialGuidedHuntReport
                            && ((SpecialGuidedHuntReport)x).ClientId == personId
                        )
                        || (
                            x is OutfitterGuidedHuntReport
                            && ((OutfitterGuidedHuntReport)x).ClientId == personId
                        )
                    ) && applicableSeasonIds.Contains(x.Season.Id)
            )
            .ToListAsync();

        reports.Add(report);

        return reports
            .SelectMany(
                report =>
                    report
                        .GetActivities()
                        .Where(y => y.Mortality.Species is Data.Enums.Species.GrizzlyBear)
                        .Select(activity => (report, activity))
            )
            .OrderBy(x => x.activity.Mortality.DateOfDeath)
            .ToArray();
    }
}
