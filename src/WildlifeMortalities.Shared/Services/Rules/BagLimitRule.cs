using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Extensions;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Shared.Services.Rules;

public class BagLimitRule : Rule
{
    public override IEnumerable<RuleType> ApplicableRuleTypes =>
        new[] { RuleType.BagLimitExceeded, RuleType.HarvestPeriod };

    private static async Task<IList<BagEntry>> GetCurrentBagCount(
        AppDbContext context,
        Season season,
        PersonWithAuthorizations person
    )
    {
        return await context.BagEntries
            .Include(x => x.BagLimitEntry)
            .ThenInclude(x => ((HuntingBagLimitEntry)x).Areas)
            .Include(x => x.BagLimitEntry)
            .ThenInclude(x => ((HuntingBagLimitEntry)x).Season)
            .Include(x => x.BagLimitEntry)
            .ThenInclude(x => ((TrappingBagLimitEntry)x).Concessions)
            .Include(x => x.BagLimitEntry)
            .ThenInclude(x => ((TrappingBagLimitEntry)x).Season)
            .Include(x => x.BagLimitEntry)
            .ThenInclude(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => x.Mortality)
            .Include(x => x.BagLimitEntry)
            .ThenInclude(x => x.MaxValuePerPersonSharedWith)
            .ThenInclude(x => x.ActivityQueue)
            .ThenInclude(x => x.Activity)
            .ThenInclude(x => x.Mortality)
            .Where(x => x.Person == person)
            .Where(
                x =>
                    season.Id
                    == (
                        x.BagLimitEntry is HuntingBagLimitEntry
                            ? ((HuntingBagLimitEntry)x.BagLimitEntry).Season.Id
                            : ((TrappingBagLimitEntry)x.BagLimitEntry).Season.Id
                    )
            )
            .AsSplitQuery()
            .ToListAsync();
    }

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (
            report.GeneralizedReportType
            is not GeneralizedReportType.Hunted
                and not GeneralizedReportType.Trapped
        )
        {
            return RuleResult.NotApplicable;
        }

        var violations = new List<Violation>();
        var bagEntries = await GetCurrentBagCount(context, report.Season, report.GetPerson());

        foreach (
            var activity in report
                .GetActivities()
                .OfType<HarvestActivity>()
                .OrderBy(x => x.Mortality.DateOfDeath)
        )
        {
            var bagEntry = bagEntries.SingleOrDefault(
                x => x.BagLimitEntry.Matches(activity, report)
            );
            if (bagEntry == null)
            {
                var bagLimitEntry = (
                    await context.BagLimitEntries
                        .Include(x => x.MaxValuePerPersonSharedWith)
                        .Include(x => x.ActivityQueue)
                        .ThenInclude(x => x.Activity)
                        .ThenInclude(x => x.Mortality)
                        .Include(x => ((HuntingBagLimitEntry)x).Areas)
                        .Include(x => ((HuntingBagLimitEntry)x).Season)
                        .Include(x => ((TrappingBagLimitEntry)x).Concessions)
                        .Include(x => ((TrappingBagLimitEntry)x).Season)
                        .Where(x => x.Species == activity.Mortality.Species)
                        .AsSplitQuery()
                        .ToArrayAsync()
                ).SingleOrDefault(x => x.Matches(activity, report));

                if (bagLimitEntry == null)
                {
                    violations.Add(
                        new(
                            activity,
                            RuleType.HarvestPeriod,
                            SeverityType.Illegal,
                            activity.Mortality.Sex is Sex.Unknown
                                ? $"{(activity is HuntedActivity ? "Area" : "Concession")} {activity.GetAreaName(report)} is closed to {(activity is HuntedActivity ? "hunting" : "trapping")} for {activity.Mortality.Species.GetDisplayName().ToLower()} of {activity.Mortality.Sex!.GetDisplayName().ToLower()} sex on {activity.Mortality.DateOfDeath:yyyy-MM-dd}."
                                : $"{(activity is HuntedActivity ? "Area" : "Concession")} {activity.GetAreaName(report)} is closed to {(activity is HuntedActivity ? "hunting" : "trapping")} for {activity.Mortality.Sex!.GetDisplayName().ToLower()} {activity.Mortality.Species.GetDisplayName().ToLower()} on {activity.Mortality.DateOfDeath:yyyy-MM-dd}."
                        )
                    );
                    continue;
                }

                bagEntry = new BagEntry
                {
                    BagLimitEntry = bagLimitEntry,
                    Person = report.GetPerson(),
                };

                context.BagEntries.Add(bagEntry);
                bagEntries.Add(bagEntry);
            }

            var isLimitExceeded = bagEntry.Increase(context, activity, bagEntries);

            if (isLimitExceeded)
            {
                violations.Add(
                    new(
                        activity,
                        RuleType.BagLimitExceeded,
                        SeverityType.Illegal,
                        $"Bag limit exceeded for {string.Join(" and ", bagEntry.GetSpeciesDescriptions())} in {(activity is HuntedActivity ? "area" : "concession")} {activity.GetAreaName(report)} for {bagEntry.BagLimitEntry.GetSeason()} season."
                    )
                );
            }
        }

        return violations.Count == 0 ? RuleResult.IsLegal : RuleResult.IsIllegal(violations);
    }

    public override async Task Reset(Report report, AppDbContext context)
    {
        if (
            report.GeneralizedReportType
            is not GeneralizedReportType.Hunted
                and not GeneralizedReportType.Trapped
        )
        {
            return;
        }

        var bagEntries = await GetCurrentBagCount(context, report.Season, report.GetPerson());
        foreach (
            var activity in report
                .GetActivities()
                .OfType<HarvestActivity>()
                .OrderBy(x => x.Mortality.DateOfDeath)
        )
        {
            var bagEntry = bagEntries.SingleOrDefault(
                x => x.BagLimitEntry.Matches(activity, report)
            );

            if (bagEntry == null)
            {
                continue;
            }

            if (bagEntry.BagLimitEntry.ActivityQueue.Any(x => x.Activity == activity))
            {
                bagEntry.Decrease(activity, bagEntries);
            }
        }
    }
}
