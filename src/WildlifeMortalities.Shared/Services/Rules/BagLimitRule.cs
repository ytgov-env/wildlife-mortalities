using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;

namespace WildlifeMortalities.Shared.Services.Rules;

public class BagLimitRule : Rule
{
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
            .ThenInclude(x => ((TrappingBagLimitEntry)x).Concessions)
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
            var bagEntry = bagEntries.FirstOrDefault(
                x => x.BagLimitEntry.Matches(activity, report)
            );
            if (bagEntry == null)
            {
                var entry = (
                    await context.BagLimitEntries
                        .Include(x => x.MaxValuePerPersonSharedWith)
                        .Include(x => x.ActivityQueue)
                        .ThenInclude(x => x.Activity)
                        .ThenInclude(x => x.Mortality)
                        .Include(x => ((HuntingBagLimitEntry)x).Areas)
                        .Include(x => ((TrappingBagLimitEntry)x).Concessions)
                        .Where(x => x.Species == activity.Mortality.Species)
                        .AsSplitQuery()
                        .ToArrayAsync()
                ).FirstOrDefault(x => x.Matches(activity, report));

                if (entry == null)
                {
                    violations.Add(Violation.IllegalHarvestPeriod(activity, report));
                    continue;
                }

                bagEntry = new BagEntry { BagLimitEntry = entry, Person = report.GetPerson(), };

                context.BagEntries.Add(bagEntry);
                bagEntries.Add(bagEntry);
            }

            var isLimitExceeded = bagEntry.Increase(context, activity, bagEntries);

            if (isLimitExceeded)
            {
                violations.Add(Violation.BagLimitExceeded(activity, report, bagEntry));
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
            var bagEntry = bagEntries.FirstOrDefault(
                x => x.BagLimitEntry.Matches(activity, report)
            );

            if (bagEntry == null)
            {
                continue;
            }

            bagEntry.Decrease(activity, bagEntries);
        }
    }
}
