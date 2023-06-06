using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;

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

    public static async Task<IList<BagLimitEntryPerPerson>> GetCurrentBagCount(
        AppDbContext context,
        Season season,
        PersonWithAuthorizations person
    )
    {
        return await context.BagLimitEntriesPerPerson
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
        foreach (var item in report.GetActivities().OfType<HuntedActivity>())
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
                            Rule = Violation.RuleType.BagLimit,
                            Description =
                                $"Bag limit has not been configured for {item.Mortality.Species} in {item.GameManagementArea} for {report.Season} season. Please report to service desk.",
                            Severity = Violation.ViolationSeverity.InternalError
                        }
                    );
                    continue;
                }

                personalEntry = new BagLimitEntryPerPerson
                {
                    BagLimitEntry = entry,
                    Person = report.GetPerson(),
                };

                context.BagLimitEntriesPerPerson.Add(personalEntry);
                personalEntries.Add(personalEntry);
            }

            if (personalEntry.Total + 1 > personalEntry.BagLimitEntry.MaxValue)
            {
                violations.Add(
                    new Violation
                    {
                        Description =
                            $"Bag limit exceeded for {personalEntry.BagLimitEntry.Species} in {personalEntry.BagLimitEntry.Area} for {personalEntry.BagLimitEntry.Season} season",
                        //Severity = ViolationSeverity.Major
                    }
                );
            }

            personalEntry.Increase(context, personalEntries);
        }

        return violations.Count == 0 ? RuleResult.IsLegal : RuleResult.IsIllegal(violations);
    }
}
