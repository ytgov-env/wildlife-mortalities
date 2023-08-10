using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Test.Helpers;
using WildlifeMortalities.Shared.Services.Rules;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Test.Unit.Rules.BagLimit;

public class CaribouTester
{
    private static AppDbContext GetContext() => TestDbContextFactory.CreateContext();

    private static (
        IndividualHuntedMortalityReport,
        IndividualHuntedMortalityReport,
        IndividualHuntedMortalityReport,
        IndividualHuntedMortalityReport,
        IndividualHuntedMortalityReport
    ) GetReports(AppDbContext context)
    {
        var person = new Client { Id = 4 };
        var season = new HuntingSeason(2023);
        context.People.Add(person);

        var overlapArea = new GameManagementArea
        {
            Zone = "2",
            Subzone = "16",
            Id = 10,
        };

        var porcupineOnlyArea = new GameManagementArea
        {
            Zone = "2",
            Subzone = "15",
            Id = 11,
        };

        var onlyPorcupineEntry = new CaribouBagLimitEntry(
            new[] { porcupineOnlyArea },
            season,
            season.StartDate,
            season.EndDate,
            2
        );

        var hartRiverEntry = new CaribouBagLimitEntry(
            new[] { overlapArea },
            season,
            new DateTimeOffset(new DateTime(2023, 8, 1)),
            new DateTimeOffset(new DateTime(2023, 10, 30)),
            1
        );

        var porcupineWinterEntry = new CaribouBagLimitEntry(
            new[] { overlapArea },
            season,
            new DateTimeOffset(new DateTime(2023, 11, 1)),
            new DateTimeOffset(new DateTime(2024, 1, 31)),
            2
        );

        onlyPorcupineEntry.MaxValuePerPersonSharedWith.Add(hartRiverEntry);
        onlyPorcupineEntry.MaxValuePerPersonSharedWith.Add(porcupineWinterEntry);

        hartRiverEntry.MaxValuePerPersonSharedWith.Add(onlyPorcupineEntry);
        hartRiverEntry.MaxValuePerPersonSharedWith.Add(porcupineWinterEntry);

        porcupineWinterEntry.MaxValuePerPersonSharedWith.Add(onlyPorcupineEntry);
        porcupineWinterEntry.MaxValuePerPersonSharedWith.Add(hartRiverEntry);

        context.GameManagementAreas.AddRange(porcupineOnlyArea, overlapArea);
        context.BagLimitEntries.AddRange(onlyPorcupineEntry, hartRiverEntry, porcupineWinterEntry);

        var porcupineSummerReport = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = season.StartDate.AddDays(2),
                    LegalHerd = CaribouMortality.CaribouHerd.Porcupine,
                    Sex = Data.Enums.Sex.Male
                },
                GameManagementArea = porcupineOnlyArea,
            },
            Season = season,
            Person = person,
        };

        var hartRiverReport = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(new DateTime(2023, 8, 20)),
                    Sex = Data.Enums.Sex.Male
                },
                GameManagementArea = overlapArea,
            },
            Season = season,
            Person = person,
        };

        var secondHartRiverReport = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(new DateTime(2023, 8, 28)),
                    Sex = Data.Enums.Sex.Male
                },
                GameManagementArea = overlapArea,
            },
            Season = season,
            Person = person,
        };

        var porcupineWinterReport = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(new DateTime(2023, month: 11, 10)),
                    LegalHerd = CaribouMortality.CaribouHerd.Porcupine,
                    Sex = Data.Enums.Sex.Male
                },
                GameManagementArea = overlapArea,
            },
            Season = season,
            Person = person,
        };

        var secondPorcupineWinterReport = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(new DateTime(2023, 11, 14)),
                    Sex = Data.Enums.Sex.Male
                },
                GameManagementArea = overlapArea,
            },
            Season = season,
            Person = person,
        };

        context.Reports.AddRange(
            porcupineSummerReport,
            porcupineWinterReport,
            secondPorcupineWinterReport,
            hartRiverReport,
            secondHartRiverReport
        );

        context.SaveChanges();

        return (
            hartRiverReport,
            secondHartRiverReport,
            porcupineSummerReport,
            porcupineWinterReport,
            secondPorcupineWinterReport
        );
    }

    [Theory]
    [InlineData("PPW", true)]
    [InlineData("PPP", true)]
    [InlineData("PWP", true)]
    [InlineData("WPP", true)]
    [InlineData("WW", true)]
    [InlineData("PP", false)]
    [InlineData("PW", false)]
    [InlineData("WP", false)]
    [InlineData("W", false)]
    public async Task Process_WithParameterizedInputs(string sequence, bool shouldBeViolation)
    {
        var context = GetContext();

        var (
            hartRiverReport,
            secondHartRiverReport,
            porcupineSummerReport,
            porcupineAutumnReport,
            secondPorcupineReport
        ) = GetReports(context);

        var availablePorcupineReports = new List<IndividualHuntedMortalityReport>
        {
            porcupineSummerReport,
            porcupineAutumnReport,
            secondPorcupineReport,
        };

        var availablHartRiverReports = new List<IndividualHuntedMortalityReport>
        {
            hartRiverReport,
            secondHartRiverReport,
        };

        var report = sequence
            .Select(x =>
            {
                var listToChoose = x == 'P' ? availablePorcupineReports : availablHartRiverReports;

                var report = listToChoose.First();
                listToChoose.RemoveAt(0);

                return report;
            })
            .ToList();

        var rule = new BagLimitRule();

        for (var i = 0; i < sequence.Length; i++)
        {
            var reportToTest = report[i];
            var result = await rule.Process(reportToTest, context);

            await context.SaveChangesAsync();
            result.IsApplicable.Should().BeTrue();

            if (i < sequence.Length - 1)
                result.Violations.Should().BeEmpty();
            else
            {
                if (!shouldBeViolation)
                {
                    result.Violations.Should().BeEmpty();
                    continue;
                }

                result.Violations.Should().ContainSingle();

                var violation = result.Violations.First();

                violation.Description
                    .Should()
                    .BeEquivalentTo(
                        "Bag limit exceeded for caribou in area 2-16 for 23/24 season."
                    );
                violation.Activity.Should().Be(reportToTest.Activity);
                violation.Severity.Should().Be(SeverityType.Illegal);
                violation.Rule.Should().Be(RuleType.BagLimitExceeded);
            }
        }
    }
}
