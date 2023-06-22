using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data.Entities;

namespace WildlifeMortalities.Test.Rules.BagLimit;

public class MatchesTester
{
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
            Areas = new() { area },
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
            Areas = new() { area },
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
            Areas = new() { area },
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
            Areas = new() { area },
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
            Areas = new() { area },
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
            Areas = new() { area },
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
            Areas = new() { area },
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
