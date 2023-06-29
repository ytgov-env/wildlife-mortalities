using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data.Rules;
using WildlifeMortalities.Test.Helpers;
using Xunit;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Test.Rules.BagLimit;

public class ProcessTester
{
    private static AppDbContext GetContext() => TestDbContextFactory.GetContext();

    private static void GenerateHuntingBagLimitDefaults(
        out AppDbContext context,
        out Report report,
        Action<BagEntry, Report, AppDbContext>? personEntryModifier = null,
        Action<
            HuntingBagLimitEntry,
            PersonWithAuthorizations,
            Report,
            AppDbContext
        >? entryModifier = null,
        Func<GameManagementArea, Season, PersonWithAuthorizations, Report>? reportModifier = null
    )
    {
        context = GetContext();
        var person = new Client { Id = 4 };
        var season = new HuntingSeason(2023);

        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };

        var activity = new HuntedActivity()
        {
            Mortality = new CaribouMortality()
            {
                DateOfDeath = season.StartDate.AddDays(2),
                Herd = CaribouMortality.CaribouHerd.Atlin,
                Sex = Data.Enums.Sex.Male
            },
            GameManagementArea = area,
        };

        report =
            reportModifier?.Invoke(area, season, person)
            ?? new IndividualHuntedMortalityReport
            {
                HuntedActivity = activity,
                Season = season,
                Person = person,
            };

        var bagLimitEntry = new CaribouBagLimitEntry
        {
            Areas = new() { area },
            Herds = new() { CaribouMortality.CaribouHerd.Atlin },
            MaxValuePerPerson = 2,
            Season = season,
            SharedWithDifferentSpeciesAndOrSex = new(),
            PeriodStart = season.StartDate,
            PeriodEnd = season.EndDate
        };

        entryModifier?.Invoke(bagLimitEntry, person, report, context);

        var personalBagLimit = new BagEntry { BagLimitEntry = bagLimitEntry, Person = person, };

        personEntryModifier?.Invoke(personalBagLimit, report, context);

        context.Reports.Add(report);
        context.People.Add(person);
        context.BagLimitEntries.Add(bagLimitEntry);
        context.BagEntries.Add(personalBagLimit);

        context.SaveChanges();
    }

    [Theory]
    [InlineData(typeof(HumanWildlifeConflictMortalityReport))]
    [InlineData(typeof(CollaredMortalityReport))]
    [InlineData(typeof(ResearchMortalityReport))]
    public async Task Process_WithNotApplicableReport_ReturnsNotApplicableResult(Type type)
    {
        var report = type.GetConstructor(Array.Empty<Type>())!.Invoke(null) as Report;
        var rule = new BagLimitRule();
        using var context = new AppDbContext();

        var result = await rule.Process(report!, context);
        result.IsApplicable.Should().BeFalse();
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithOneCaribou_ReturnsLegal()
    {
        GenerateHuntingBagLimitDefaults(out var context, out var report);

        var rule = new BagLimitRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithNoEntry_ReturnsHarvestPeriodViolation()
    {
        var report = new IndividualHuntedMortalityReport
        {
            HuntedActivity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                    Herd = CaribouMortality.CaribouHerd.Atlin
                },
                GameManagementArea = new GameManagementArea
                {
                    Zone = "4",
                    Subzone = "03",
                    Id = 10,
                }
            },
            Season = new HuntingSeason(2023)
        };

        var rule = new BagLimitRule();
        var context = GetContext();

        var result = await rule.Process(report, context);
        result.Violations.Should().ContainSingle();
        var violation = result.Violations.First();

        violation.Description
            .Should()
            .BeEquivalentTo("Area 4-03 is closed to harvest for caribou on 2023-04-01.");
        violation.Activity.Should().Be(report.HuntedActivity);
        violation.Severity.Should().Be(SeverityType.Illegal);
        violation.Rule.Should().Be(RuleType.HarvestPeriod);
    }

    [Fact]
    public async Task Process_WithExceededLimit_ReturnsViolation()
    {
        GenerateHuntingBagLimitDefaults(
            out var context,
            out var report,
            personEntryModifier: (entry, report, _) =>
            {
                entry
                    .Increase(null!, (HuntedActivity)report.GetActivities().First(), null!)
                    .Should()
                    .BeFalse();
                entry
                    .Increase(null!, (HuntedActivity)report.GetActivities().First(), null!)
                    .Should()
                    .BeFalse();
            }
        );

        var rule = new BagLimitRule();
        var result = await rule.Process(report, context);
        result.Violations.Should().ContainSingle();
        var violation = result.Violations.First();

        violation.Description
            .Should()
            .BeEquivalentTo("Bag limit exceeded for Caribou in 4-03 for 23/24 season.");
        violation.Activity.Should().Be(report.GetActivities().First());
        violation.Severity.Should().Be(SeverityType.Illegal);
        violation.Rule.Should().Be(RuleType.BagLimit);
    }

    [Fact]
    public async Task Process_WithExceededLimitFromShared_ReturnsViolation()
    {
        GenerateHuntingBagLimitDefaults(
            out var context,
            out var report,
            entryModifier: (entry, person, report, context) =>
            {
                var otherBagEntry = new HuntingBagLimitEntry
                {
                    Species = Data.Enums.Species.AmericanBlackBear,
                    MaxValuePerPerson = 1,
                    Sex = Data.Enums.Sex.Male,
                    SharedWithDifferentSpeciesAndOrSex = new(),
                    Season = (HuntingSeason)entry.GetSeason(),
                    Areas = entry.Areas.ToList(),
                };

                var otherPersonalBagEntry = new BagEntry
                {
                    BagLimitEntry = otherBagEntry,
                    Person = person,
                };

                otherPersonalBagEntry.Increase(
                    null!,
                    (HuntedActivity)report.GetActivities().First(),
                    null!
                );

                entry.SharedWithDifferentSpeciesAndOrSex = new() { otherBagEntry };

                context.BagLimitEntries.Add(otherBagEntry);
                context.BagEntries.Add(otherPersonalBagEntry);
            }
        );

        var rule = new BagLimitRule();
        var result = await rule.Process(report, context);
        result.Violations.Should().ContainSingle();
        var violation = result.Violations.First();

        violation.Description
            .Should()
            .BeEquivalentTo(
                "Bag limit exceeded for Caribou and Black bear in 4-03 for 23/24 season."
            );
        violation.Activity.Should().Be(report.GetActivities().First());
        violation.Severity.Should().Be(SeverityType.Illegal);
        violation.Rule.Should().Be(RuleType.BagLimit);
    }

    [Fact]
    public async Task Process_WithSharedNotExceedingLimit_ReturnsNoViolation()
    {
        GenerateHuntingBagLimitDefaults(
            out var context,
            out var report,
            entryModifier: (entry, _, report, context) =>
            {
                var otherBagEntry = new HuntingBagLimitEntry
                {
                    Species = Data.Enums.Species.AmericanBlackBear,
                    MaxValuePerPerson = 2,
                    Sex = Data.Enums.Sex.Male,
                    SharedWithDifferentSpeciesAndOrSex = new(),
                };

                var otherPersonalBagEntry = new BagEntry { BagLimitEntry = otherBagEntry, };

                otherPersonalBagEntry.Increase(
                    null!,
                    (HuntedActivity)report.GetActivities().First(),
                    null!
                );

                entry.SharedWithDifferentSpeciesAndOrSex = new() { otherBagEntry };

                context.BagLimitEntries.Add(otherBagEntry);
                context.BagEntries.Add(otherPersonalBagEntry);
            }
        );

        var rule = new BagLimitRule();
        var result = await rule.Process(report, context);
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithSharedNotExceedingLimit_AddNewPersonBagLimitEntry_ReturnsNoViolation()
    {
        GenerateHuntingBagLimitDefaults(
            out var context,
            out var report,
            entryModifier: (entry, _, _, context) =>
            {
                var otherBagEntry = new HuntingBagLimitEntry
                {
                    Species = Data.Enums.Species.AmericanBlackBear,
                    MaxValuePerPerson = 2,
                    Sex = Data.Enums.Sex.Male,
                    SharedWithDifferentSpeciesAndOrSex = new(),
                };

                entry.SharedWithDifferentSpeciesAndOrSex = new() { otherBagEntry };

                context.BagLimitEntries.Add(otherBagEntry);
            }
        );

        var rule = new BagLimitRule();
        var result = await rule.Process(report, context);
        result.Violations.Should().BeEmpty();

        context.SaveChanges();
        var personalEntry = context.BagEntries.FirstOrDefault(
            x => x.BagLimitEntry.Species == Data.Enums.Species.AmericanBlackBear
        );
        personalEntry.Should().NotBeNull();

        personalEntry!.SharedValue.Should().Be(1);
        personalEntry!.CurrentValue.Should().Be(0);
        personalEntry!.TotalValue.Should().Be(1);
    }

    [Fact]
    public async Task Process_WithExceededLimitOnSecondActivity_ReturnsViolation()
    {
        GenerateHuntingBagLimitDefaults(
            out var context,
            out var report,
            reportModifier: (area, season, person) =>
                new SpecialGuidedHuntReport()
                {
                    Client = (Client)person,
                    HuntedActivities = new List<HuntedActivity>
                    {
                        new HuntedActivity()
                        {
                            Mortality = new CaribouMortality()
                            {
                                DateOfDeath = new DateTimeOffset(
                                    2023,
                                    4,
                                    3,
                                    0,
                                    0,
                                    0,
                                    TimeSpan.FromHours(-7)
                                ),
                                Herd = CaribouMortality.CaribouHerd.Atlin,
                                Sex = Data.Enums.Sex.Male
                            },
                            GameManagementArea = area,
                        },
                        new HuntedActivity()
                        {
                            Mortality = new CaribouMortality()
                            {
                                DateOfDeath = new DateTimeOffset(
                                    2023,
                                    4,
                                    5,
                                    0,
                                    0,
                                    0,
                                    TimeSpan.FromHours(-7)
                                ),
                                Herd = CaribouMortality.CaribouHerd.Atlin,
                                Sex = Data.Enums.Sex.Male
                            },
                            GameManagementArea = area,
                        },
                        new HuntedActivity()
                        {
                            Mortality = new CaribouMortality()
                            {
                                DateOfDeath = new DateTimeOffset(
                                    2023,
                                    4,
                                    2,
                                    0,
                                    0,
                                    0,
                                    TimeSpan.FromHours(-7)
                                ),
                                Herd = CaribouMortality.CaribouHerd.Atlin,
                                Sex = Data.Enums.Sex.Male
                            },
                            GameManagementArea = area,
                        },
                        new HuntedActivity()
                        {
                            Mortality = new CaribouMortality()
                            {
                                DateOfDeath = new DateTimeOffset(
                                    2023,
                                    4,
                                    7,
                                    0,
                                    0,
                                    0,
                                    TimeSpan.FromHours(-7)
                                ),
                                Herd = CaribouMortality.CaribouHerd.Atlin,
                                Sex = Data.Enums.Sex.Male
                            },
                            GameManagementArea = area,
                        },
                    },
                    Season = season
                }
        );

        var rule = new BagLimitRule();
        var result = await rule.Process(report, context);
        result.Violations.Should().HaveCount(2);
        {
            var violation = result.Violations.First();

            violation.Description
                .Should()
                .BeEquivalentTo("Bag limit exceeded for caribou in 4-03 for 23/24 season.");
            violation.Activity.Should().Be(report.GetActivities().ElementAt(1));
            violation.Severity.Should().Be(SeverityType.Illegal);
            violation.Rule.Should().Be(RuleType.BagLimit);
        }
        {
            var violation = result.Violations.Last();

            violation.Description
                .Should()
                .BeEquivalentTo("Bag limit exceeded for caribou in 4-03 for 23/24 season.");
            violation.Activity.Should().Be(report.GetActivities().Last());
            violation.Severity.Should().Be(SeverityType.Illegal);
            violation.Rule.Should().Be(RuleType.BagLimit);
        }

        context.BagLimitEntries.First().ActivityQueue.Should().HaveCount(4);
    }

    private static void GenerateTrappingBagLimitDefaults(
        out AppDbContext context,
        out Report report,
        Func<
            RegisteredTrappingConcession,
            Season,
            PersonWithAuthorizations,
            Report
        >? reportModifier = null
    )
    {
        context = GetContext();

        var season = new TrappingSeason(2023);
        var activity = new TrappedActivity()
        {
            Mortality = new AmericanBeaverMortality()
            {
                DateOfDeath = season.StartDate.AddDays(3),
                Sex = Data.Enums.Sex.Male
            }
        };

        var person = new Client { Id = 4 };
        var concession = new RegisteredTrappingConcession { Id = 10, Area = "15" };
        report =
            reportModifier?.Invoke(concession, season, person)
            ?? new TrappedMortalitiesReport
            {
                Client = person,
                RegisteredTrappingConcession = concession,
                Season = season,
                TrappedActivities = new() { activity }
            };

        var bagLimitEntry = new TrappingBagLimitEntry
        {
            RegisteredTrappingConcessions = new() { concession },
            MaxValuePerPerson = BagLimitEntry.InfiniteMaxValuePerPerson,
            Season = season,
            Species = Data.Enums.Species.AmericanBeaver,
            SharedWithDifferentSpeciesAndOrSex = new(),
            PeriodStart = season.StartDate.AddDays(2),
            PeriodEnd = season.EndDate.AddDays(-2)
        };

        var personalBagLimit = new BagEntry { BagLimitEntry = bagLimitEntry, Person = person, };

        context.Reports.Add(report);
        context.People.Add(person);
        context.BagLimitEntries.Add(bagLimitEntry);
        context.BagEntries.Add(personalBagLimit);

        context.SaveChanges();
    }

    [Fact]
    public async Task Process_TrappingBeaver_InSeason_ReturnsLegal()
    {
        GenerateTrappingBagLimitDefaults(out var context, out var report);

        var rule = new BagLimitRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_TrappingBeaver_OutOfSeason_ReturnsViolation()
    {
        GenerateTrappingBagLimitDefaults(
            out var context,
            out var report,
            reportModifier: (concession, season, person) =>
                new TrappedMortalitiesReport
                {
                    Client = (Client)person,
                    RegisteredTrappingConcession = concession,
                    Season = season,
                    TrappedActivities = new()
                    {
                        new TrappedActivity()
                        {
                            Mortality = new AmericanBeaverMortality()
                            {
                                DateOfDeath = season.StartDate,
                                Sex = Data.Enums.Sex.Male
                            }
                        }
                    }
                }
        );

        var rule = new BagLimitRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().ContainSingle();
        var violation = result.Violations.First();

        violation.Description
            .Should()
            .BeEquivalentTo("Area 15 is closed to harvest for beaver on 2023-07-01.");
        violation.Activity.Should().Be(report.GetActivities().First());
        violation.Severity.Should().Be(SeverityType.Illegal);
        violation.Rule.Should().Be(RuleType.HarvestPeriod);
    }
}
