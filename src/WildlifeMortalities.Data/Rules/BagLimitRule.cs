﻿using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Rules;

public class BagLimitRule : Rule
{
    private readonly Species[] _speciesWithBagLimit = new[]
    {
        Species.AmericanBlackBear,
        Species.Caribou,
        Species.DuskyGrouse,
        Species.Elk,
        Species.GreyWolf,
        Species.GrizzlyBear,
        Species.Moose,
        Species.MountainGoat,
        Species.MuleDeer,
        Species.RockPtarmigan,
        Species.RuffedGrouse,
        Species.SharpTailedGrouse,
        Species.SpruceGrouse,
        Species.ThinhornSheep,
        Species.WhiteTailedPtarmigan,
        Species.WillowPtarmigan,
        Species.Wolverine,
        Species.WoodBison
    };

    private static async Task<IList<BagEntry>> GetCurrentBagCount(
        AppDbContext context,
        Season season,
        PersonWithAuthorizations person
    )
    {
        return await context.BagEntries
            .Where(x => x.BagLimitEntry.Season == season)
            .Where(x => x.Person == person)
            .ToListAsync();
    }

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (report.GeneralizedReportType is not GeneralizedReportType.Hunted)
            return RuleResult.NotApplicable;

        var relevantActivities = report
            .GetActivities()
            .Where(x => _speciesWithBagLimit.Contains(x.Mortality.Species))
            .ToArray();
        if (relevantActivities.Length == 0)
            return RuleResult.NotApplicable;

        var violations = new List<Violation>();
        var personalEntries = await GetCurrentBagCount(context, report.Season, report.GetPerson());
        foreach (
            var item in report
                .GetActivities()
                .OfType<HuntedActivity>()
                .OrderBy(x => x.Mortality.DateOfDeath)
        )
        {
            var personalEntry = personalEntries.FirstOrDefault(
                x => x.BagLimitEntry.Matches(item, report.Season)
            );
            if (personalEntry == null)
            {
                var entry = (
                    await context.BagLimitEntries
                        .Where(
                            x =>
                                x.Season.Id == report.Season.Id
                                && x.Species == item.Mortality.Species
                        )
                        .ToArrayAsync()
                ).FirstOrDefault(x => x.Matches(item, report.Season));

                if (entry == null)
                {
                    violations.Add(
                        new Violation
                        {
                            Activity = item,
                            Rule = Violation.RuleType.HarvestPeriod,
                            Description =
                                $"Area {item.GameManagementArea} is closed to hunting for {item.Mortality.Species.GetDisplayName().ToLower()} on {item.Mortality.DateOfDeath:yyyy-MM-dd}.",
                            Severity = Violation.ViolationSeverity.Illegal
                        }
                    );
                }

                personalEntry = new BagEntry
                {
                    BagLimitEntry = entry,
                    Person = report.GetPerson(),
                };

                context.BagEntries.Add(personalEntry);
                personalEntries.Add(personalEntry);
            }

            var isLimitExceeded = personalEntry.Increase(context, personalEntries);

            if (isLimitExceeded)
            {
                violations.Add(
                    new Violation
                    {
                        Activity = item,
                        Rule = Violation.RuleType.BagLimit,
                        Description =
                            $"Bag limit exceeded for {string.Join(" and ", personalEntry.GetSpeciesDescriptions())} in {item.GameManagementArea} for {personalEntry.BagLimitEntry.Season} season.",
                        Severity = Violation.ViolationSeverity.Illegal
                    }
                );
            }
        }

        return violations.Count == 0 ? RuleResult.IsLegal : RuleResult.IsIllegal(violations);
    }
}
