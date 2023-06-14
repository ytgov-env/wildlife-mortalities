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
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Test.Rules;

public static class ThreadSafeRandom
{
    private static readonly Random _globalRandom = new Random();

    [ThreadStatic]
    private static Random? _localRandom;

    public static int Next()
    {
        if (_localRandom == null)
        {
            var seed = 0;
            lock (_globalRandom)
            {
                seed = _globalRandom.Next();
            }

            _localRandom = new Random(seed);
        }

        return _localRandom.Next();
    }
}

public class BagLimitTester
{
    private static AppDbContext GetContext()
    {
        var contextName = ThreadSafeRandom.Next().ToString();
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase(contextName);

        return new AppDbContext(builder.Options);
    }

    private static void GenerateBagLimitDefaults(
        out AppDbContext context,
        out Report report,
        Action<BagLimitEntryPerPerson, AppDbContext>? personEntryModifier = null,
        Action<BagLimitEntry, PersonWithAuthorizations, AppDbContext>? entryModifier = null,
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
            Area = area,
            Herd = CaribouMortality.CaribouHerd.Atlin,
            MaxValue = 2,
            Season = season,
            SharedWith = Array.Empty<BagLimitEntry>(),
            PeriodStart = season.StartDate,
            PeriodEnd = season.EndDate
        };

        entryModifier?.Invoke(bagLimitEntry, person, context);

        var personalBagLimit = new BagLimitEntryPerPerson
        {
            BagLimitEntry = bagLimitEntry,
            Person = person,
        };

        personEntryModifier?.Invoke(personalBagLimit, context);

        context.Reports.Add(report);
        context.People.Add(person);
        context.BagLimitEntries.Add(bagLimitEntry);
        context.BagLimitEntriesPerPerson.Add(personalBagLimit);

        context.SaveChanges();
    }

    [Theory]
    [InlineData(typeof(HumanWildlifeConflictMortalityReport))]
    [InlineData(typeof(TrappedMortalitiesReport))]
    [InlineData(typeof(CollaredMortalityReport))]
    [InlineData(typeof(ResearchMortalityReport))]
    public async Task Process_WithNotApplicableReport_ReturnsNotApplicableResult(Type type)
    {
        var report = type.GetConstructor(Array.Empty<Type>())!.Invoke(null) as Report;
        var rule = new BagLimitRule();
        using var context = new AppDbContext();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeFalse();
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithOneCaribou_ReturnsLegal()
    {
        GenerateBagLimitDefaults(out var context, out var report);

        var rule = new BagLimitRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithUnconfiguredEntry_ReturnsViolation()
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
            .BeEquivalentTo(
                "Bag limit has not been configured for Caribou in 4-03 for 23/24 season. Please report to service desk."
            );
        violation.Activity.Should().Be(report.HuntedActivity);
        violation.Severity.Should().Be(ViolationSeverity.InternalError);
        violation.Rule.Should().Be(RuleType.BagLimit);
    }

    [Fact]
    public async Task Process_WithExceededLimit_ReturnsViolation()
    {
        GenerateBagLimitDefaults(
            out var context,
            out var report,
            personEntryModifier: (entry, _) =>
            {
                entry.Increase(null!, null!).Should().BeFalse();
                entry.Increase(null!, null!).Should().BeFalse();
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
        violation.Severity.Should().Be(ViolationSeverity.Illegal);
        violation.Rule.Should().Be(RuleType.BagLimit);
    }

    [Fact]
    public async Task Process_WithExceededLimitFromShared_ReturnsViolation()
    {
        GenerateBagLimitDefaults(
            out var context,
            out var report,
            entryModifier: (entry, person, context) =>
            {
                var otherBagEntry = new BagLimitEntry
                {
                    Species = Data.Enums.Species.AmericanBlackBear,
                    MaxValue = 1,
                    Sex = Data.Enums.Sex.Male,
                    SharedWith = Array.Empty<BagLimitEntry>(),
                    Season = entry.Season,
                    Area = entry.Area,
                };

                var otherPersonalBagEntry = new BagLimitEntryPerPerson
                {
                    BagLimitEntry = otherBagEntry,
                    Person = person,
                };

                otherPersonalBagEntry.Increase(null!, null!);

                entry.SharedWith = new[] { otherBagEntry };

                context.BagLimitEntries.Add(otherBagEntry);
                context.BagLimitEntriesPerPerson.Add(otherPersonalBagEntry);
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
        violation.Severity.Should().Be(ViolationSeverity.Illegal);
        violation.Rule.Should().Be(RuleType.BagLimit);
    }

    [Fact]
    public async Task Process_WithSharedNotExceedingLimit_ReturnsNoViolation()
    {
        GenerateBagLimitDefaults(
            out var context,
            out var report,
            entryModifier: (entry, _, context) =>
            {
                var otherBagEntry = new BagLimitEntry
                {
                    Species = Data.Enums.Species.AmericanBlackBear,
                    MaxValue = 2,
                    Sex = Data.Enums.Sex.Male,
                    SharedWith = Array.Empty<BagLimitEntry>(),
                };

                var otherPersonalBagEntry = new BagLimitEntryPerPerson
                {
                    BagLimitEntry = otherBagEntry,
                };

                otherPersonalBagEntry.Increase(null!, null!);

                entry.SharedWith = new[] { otherBagEntry };

                context.BagLimitEntries.Add(otherBagEntry);
                context.BagLimitEntriesPerPerson.Add(otherPersonalBagEntry);
            }
        );

        var rule = new BagLimitRule();
        var result = await rule.Process(report, context);
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithSharedNotExceedingLimit_AddNewPersonBagLimitEntry_AND_ReturnsNoViolation()
    {
        GenerateBagLimitDefaults(
            out var context,
            out var report,
            entryModifier: (entry, _, context) =>
            {
                var otherBagEntry = new BagLimitEntry
                {
                    Species = Data.Enums.Species.AmericanBlackBear,
                    MaxValue = 2,
                    Sex = Data.Enums.Sex.Male,
                    SharedWith = Array.Empty<BagLimitEntry>(),
                };

                entry.SharedWith = new[] { otherBagEntry };

                context.BagLimitEntries.Add(otherBagEntry);
            }
        );

        var rule = new BagLimitRule();
        var result = await rule.Process(report, context);
        result.Violations.Should().BeEmpty();

        context.SaveChanges();
        var personalEntry = context.BagLimitEntriesPerPerson.FirstOrDefault(
            x => x.BagLimitEntry.Species == Data.Enums.Species.AmericanBlackBear
        );
        personalEntry.Should().NotBeNull();

        personalEntry!.SharedValue.Should().Be(1);
        personalEntry!.CurrentValue.Should().Be(0);
        personalEntry!.Total.Should().Be(1);
    }

    [Fact]
    public async Task Process_WithExceededLimitOnSecondActivity_ReturnsViolation()
    {
        GenerateBagLimitDefaults(
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
            violation.Severity.Should().Be(ViolationSeverity.Illegal);
            violation.Rule.Should().Be(RuleType.BagLimit);
        }
        {
            var violation = result.Violations.Last();

            violation.Description
                .Should()
                .BeEquivalentTo("Bag limit exceeded for caribou in 4-03 for 23/24 season.");
            violation.Activity.Should().Be(report.GetActivities().Last());
            violation.Severity.Should().Be(ViolationSeverity.Illegal);
            violation.Rule.Should().Be(RuleType.BagLimit);
        }
    }

    [Fact]
    public void Matches_WithMatching_ReturnsTrue()
    {
        var season = new HuntingSeason(2023);
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new BagLimitEntry
        {
            Area = area,
            Season = season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose,
            PeriodStart = season.StartDate,
            PeriodEnd = season.EndDate
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality
            {
                Sex = Data.Enums.Sex.Male,
                DateOfDeath = season.StartDate.AddDays(2)
            },
            GameManagementArea = area,
        };

        entry.Matches(activity, season).Should().BeTrue();
    }

    [Theory]
    [InlineData(Data.Enums.Sex.Male)]
    [InlineData(Data.Enums.Sex.Female)]
    [InlineData(Data.Enums.Sex.Unknown)]
    public void Matches_WithMatching_NoSexSpecified_ReturnsTrue(Data.Enums.Sex sex)
    {
        var season = new HuntingSeason(2023);
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new BagLimitEntry
        {
            Area = area,
            Season = season,
            Sex = null,
            Species = Data.Enums.Species.Moose,
            PeriodStart = season.StartDate,
            PeriodEnd = season.EndDate
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality
            {
                Sex = sex,
                DateOfDeath = season.StartDate.AddDays(20)
            },
            GameManagementArea = area,
        };

        entry.Matches(activity, season).Should().BeTrue();
    }

    [Fact]
    public void Matches_WithDifferentSex_ReturnsFalse()
    {
        var season = new HuntingSeason(2023);
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new BagLimitEntry
        {
            Area = area,
            Season = season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose,
            PeriodStart = season.StartDate,
            PeriodEnd = season.EndDate
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality
            {
                Sex = Data.Enums.Sex.Female,
                DateOfDeath = season.StartDate.AddDays(2)
            },
            GameManagementArea = area,
        };

        entry.Matches(activity, season).Should().BeFalse();
    }

    [Fact]
    public void Matches_WithDifferentSpecies_ReturnsFalse()
    {
        var season = new HuntingSeason(2023);
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new BagLimitEntry
        {
            Area = area,
            Season = season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose
        };

        var activity = new HuntedActivity
        {
            Mortality = new CaribouMortality { Sex = Data.Enums.Sex.Male },
            GameManagementArea = area,
        };

        entry.Matches(activity, season).Should().BeFalse();
    }

    [Fact]
    public void Matches_WithDifferentSeason_ReturnsFalse()
    {
        var season = new HuntingSeason(2023) { Id = 1 };
        var season2 = new HuntingSeason(2024) { Id = 2 };
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new BagLimitEntry
        {
            Area = area,
            Season = season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality { Sex = Data.Enums.Sex.Male },
            GameManagementArea = area,
        };

        entry.Matches(activity, season2).Should().BeFalse();
    }

    [Fact]
    public void Matches_WithDifferentArea_ReturnsFalse()
    {
        var season = new HuntingSeason(2023);
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new BagLimitEntry
        {
            Area = area,
            Season = season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality { Sex = Data.Enums.Sex.Male },
            GameManagementArea = new GameManagementArea
            {
                Zone = "4",
                Subzone = "02",
                Id = 9
            },
        };

        entry.Matches(activity, season).Should().BeFalse();
    }

    [Fact]
    public void Matches_WithDifferentPeriod_ReturnsFalse()
    {
        var season = new HuntingSeason(2023);
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new BagLimitEntry
        {
            Area = area,
            Season = season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose,
            PeriodStart = season.StartDate.AddDays(1),
            PeriodEnd = season.EndDate.AddDays(-1)
        };

        {
            var activity = new HuntedActivity
            {
                Mortality = new MooseMortality
                {
                    Sex = Data.Enums.Sex.Male,
                    DateOfDeath = entry.PeriodStart.AddDays(-1)
                },
                GameManagementArea = area,
            };

            entry.Matches(activity, season).Should().BeFalse();
        }
        {
            var activity = new HuntedActivity
            {
                Mortality = new MooseMortality
                {
                    Sex = Data.Enums.Sex.Male,
                    DateOfDeath = entry.PeriodEnd.AddDays(1)
                },
                GameManagementArea = area,
            };

            entry.Matches(activity, season).Should().BeFalse();
        }
    }
}
