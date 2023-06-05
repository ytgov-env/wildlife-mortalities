using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using static WildlifeMortalities.Data.Entities.Mortalities.CaribouMortality;

namespace WildlifeMortalities.Data.Entities;

public class ViolationReport
{
    public int Id { get; set; }
    public Report Report { get; set; } = null!;
    public List<Violation> Violations { get; set; } = null!;
    public List<Authorization> Authorizations { get; set; } = null!;

    public static async Task<ViolationReport> Generate(Report report, AppDbContext context)
    {
        var violationReport = new ViolationReport();
        var rules = RuleEngine.Rules;
        foreach (var item in rules)
        {
            var result = await item.Process(report, context);
            violationReport.Violations.AddRange(result.Violations);
            violationReport.Authorizations.AddRange(result.Authorizations);
        }

        return violationReport;
    }
}

public static class RuleEngine
{
    public static IEnumerable<Rule> Rules { get; } = new List<Rule> { new BagLimitRule(), };
}

public class RuleResult
{
    public IEnumerable<Violation> Violations { get; private set; } = Array.Empty<Violation>();
    public IEnumerable<Authorization> Authorizations { get; private set; } =
        Array.Empty<Authorization>();

    public bool IsApplicable { get; private set; } = true;

    private RuleResult() { }

    public static RuleResult NotApplicable => new() { IsApplicable = false };
    public static RuleResult IsLegal => new();

    public static RuleResult IsIllegal(IEnumerable<Violation> violations) =>
        new() { Violations = violations };
}

public abstract class Rule
{
    public abstract Task<RuleResult> Process(Report report, AppDbContext context);
}

public class BagLimitRule : Rule
{
    public class BagLimitEntryPerPerson
    {
        public int CurrentValue { get; private set; }
        public int SharedCounter { get; private set; }
        public int Total => CurrentValue + SharedCounter;

        public PersonWithAuthorizations Person { get; init; } = null!;
        public BagLimitEntry BagLimitEntry { get; init; } = null!;

        internal void Increase(
            AppDbContext context,
            ICollection<BagLimitEntryPerPerson> personalEntries
        )
        {
            CurrentValue++;

            foreach (var shared in BagLimitEntry.SharedWith)
            {
                var existingSharedEntry = personalEntries.FirstOrDefault(
                    x => x.BagLimitEntry == shared
                );
                if (existingSharedEntry != null)
                {
                    existingSharedEntry.SharedCounter++;
                }
                else
                {
                    var entry = new BagLimitEntryPerPerson
                    {
                        BagLimitEntry = shared,
                        Person = Person,
                        SharedCounter = 1
                    };

                    personalEntries.Add(entry);
                    context.BagLimitEntriesPerPerson.Add(entry);
                }
            }
        }
    }

    public class BagLimitEntry
    {
        public GameManagementArea Area { get; set; } = null!;
        public Species Species { get; set; }
        public Sex? Sex { get; set; }
        public Season Season { get; set; } = null!;
        public IEnumerable<BagLimitEntry> SharedWith { get; set; } = null!;

        public int MaxValue { get; set; }

        public virtual bool Matches(HuntedActivity activity, Season season)
        {
            if (
                Area.Id != activity.GameManagementArea.Id
                || Species != activity.Mortality.Species
                || Season.Id != season.Id
                || (Sex.HasValue && Sex != activity.Mortality.Sex)
            )
            {
                return false;
            }
            return true;
        }
    }

    public class CaribouBagLimitEntry : BagLimitEntry
    {
        public CaribouHerd Herd { get; set; }

        override public bool Matches(HuntedActivity activity, Season season)
        {
            var baseResult = base.Matches(activity, season);
            if (!baseResult)
            {
                return false;
            }

            if (activity.Mortality is not CaribouMortality caribouMortality)
            {
                return false;
            }

            return caribouMortality.Herd == Herd;
        }
    }

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
        {
            return RuleResult.NotApplicable;
        }

        var relevantActivities = report
            .GetActivities()
            .Where(x => _speciesWithBagLimit.Contains(x.Mortality.Species))
            .ToArray();
        if (relevantActivities.Length == 0)
        {
            return RuleResult.NotApplicable;
        }

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
