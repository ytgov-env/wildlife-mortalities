using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Entities.Seasons;
using Xunit;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Test.Helpers;
using WildlifeMortalities.Shared.Services.Rules;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Test.Rules.BagLimit;

public class CaribouTester
{
    private static AppDbContext GetContext() => TestDbContextFactory.CreateContext();

    private static (
        IndividualHuntedMortalityReport,
        IndividualHuntedMortalityReport,
        IndividualHuntedMortalityReport
    ) GetReports(AppDbContext context)
    {
        var person = new Client { Id = 4 };
        var season = new HuntingSeason(2023);
        context.People.Add(person);

        var porcupineWinterArea = new GameManagementArea
        {
            Zone = "2",
            Subzone = "16",
            Id = 10,
        };

        var area = new GameManagementArea
        {
            Zone = "2",
            Subzone = "15",
            Id = 11,
        };

        var onlyporcupineEntry = new CaribouBagLimitEntry(
            new[] { porcupineWinterArea },
            new[] { CaribouMortality.CaribouHerd.Porcupine },
            season,
            season.StartDate,
            season.EndDate,
            2
        );

        var hartRiverEntry = new CaribouBagLimitEntry(
            new[] { area },
            new[] { CaribouMortality.CaribouHerd.HartRiver },
            season,
            new DateTimeOffset(new DateTime(2023, 8, 1)),
            new DateTimeOffset(new DateTime(2023, 10, 30)),
            2
        );

        var porcupineAutumnEntry = new CaribouBagLimitEntry(
            new[] { area },
            new[] { CaribouMortality.CaribouHerd.Porcupine },
            season,
            new DateTimeOffset(new DateTime(2023, 11, 1)),
            new DateTimeOffset(new DateTime(2024, 1, 31)),
            2
        );

        onlyporcupineEntry.MaxValuePerPersonSharedWith.Add(hartRiverEntry);
        onlyporcupineEntry.MaxValuePerPersonSharedWith.Add(porcupineAutumnEntry);

        hartRiverEntry.MaxValuePerPersonSharedWith.Add(onlyporcupineEntry);
        hartRiverEntry.MaxValuePerPersonSharedWith.Add(porcupineAutumnEntry);

        porcupineAutumnEntry.MaxValuePerPersonSharedWith.Add(onlyporcupineEntry);
        porcupineAutumnEntry.MaxValuePerPersonSharedWith.Add(hartRiverEntry);

        context.GameManagementAreas.AddRange(area, porcupineWinterArea);
        context.BagLimitEntries.AddRange(onlyporcupineEntry, hartRiverEntry, porcupineAutumnEntry);

        var porcupineSummerActivity = new HuntedActivity()
        {
            Mortality = new CaribouMortality()
            {
                DateOfDeath = season.StartDate.AddDays(2),
                Herd = CaribouMortality.CaribouHerd.Porcupine,
                Sex = Data.Enums.Sex.Male
            },
            GameManagementArea = porcupineWinterArea,
        };

        var porcupineSummerReport = new IndividualHuntedMortalityReport
        {
            HuntedActivity = porcupineSummerActivity,
            Season = season,
            Person = person,
        };

        var hartRiverActivity = new HuntedActivity()
        {
            Mortality = new CaribouMortality()
            {
                DateOfDeath = new DateTimeOffset(new DateTime(2023, 8, 20)),
                Herd = CaribouMortality.CaribouHerd.HartRiver,
                Sex = Data.Enums.Sex.Male
            },
            GameManagementArea = area,
        };

        var hartRiverReport = new IndividualHuntedMortalityReport
        {
            HuntedActivity = hartRiverActivity,
            Season = season,
            Person = person,
        };

        var porcupineAutumnActivity = new HuntedActivity()
        {
            Mortality = new CaribouMortality()
            {
                DateOfDeath = new DateTimeOffset(new DateTime(2023, 11, 10)),
                Herd = CaribouMortality.CaribouHerd.Porcupine,
                Sex = Data.Enums.Sex.Male
            },
            GameManagementArea = area,
        };

        var porcupineAutumnReport = new IndividualHuntedMortalityReport
        {
            HuntedActivity = porcupineAutumnActivity,
            Season = season,
            Person = person,
        };

        context.Reports.AddRange(porcupineSummerReport, hartRiverReport, porcupineAutumnReport);

        context.SaveChanges();

        return (hartRiverReport, porcupineSummerReport, porcupineAutumnReport);
    }

    [Theory]
    [InlineData("PPW", true)]
    [InlineData("PPP", true)]
    [InlineData("PWP", true)]
    [InlineData("WPP", true)]
    public async Task Process_WithParameterizedInputs(string sequence, bool shouldBeViolation)
    {
        var context = GetContext();

        var (hartRiverReport, porcupineSummerReport, porcupineAutumnReport) = GetReports(context);

        var availablePorcupineReports = new List<IndividualHuntedMortalityReport>
        {
            porcupineSummerReport,
            porcupineAutumnReport
        };

        var report = sequence
            .Select(x =>
            {
                IndividualHuntedMortalityReport report;
                if (x == 'P')
                {
                    report = availablePorcupineReports.First();
                    availablePorcupineReports.RemoveAt(0);
                }
                else
                {
                    report = hartRiverReport;
                }

                return report;
            })
            .ToList();

        var rule = new BagLimitRule();

        for (int i = 0; i < sequence.Length; i++)
        {
            var reportToTest = report[i];
            var result = await rule.Process(reportToTest, context);

            await context.SaveChangesAsync();
            result.IsApplicable.Should().BeTrue();

            if (i < sequence.Length - 1)
            {
                result.Violations.Should().BeEmpty();
            }
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
                    .BeEquivalentTo("Bag limit exceeded for caribou in 4-04 for 23/24 season.");
                violation.Activity.Should().Be(reportToTest.HuntedActivity);
                violation.Severity.Should().Be(SeverityType.Illegal);
                violation.Rule.Should().Be(RuleType.BagLimitExceeded);
            }
        }
    }
}
