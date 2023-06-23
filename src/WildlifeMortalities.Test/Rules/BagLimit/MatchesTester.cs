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
        var report = new IndividualHuntedMortalityReport() { Season = new HuntingSeason(2023) };
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new HuntingBagLimitEntry
        {
            Areas = new() { area },
            Season = (HuntingSeason)report.Season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose,
            PeriodStart = report.Season.StartDate,
            PeriodEnd = report.Season.EndDate
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality
            {
                Sex = Data.Enums.Sex.Male,
                DateOfDeath = report.Season.StartDate.AddDays(2)
            },
            GameManagementArea = area,
        };

        entry.Matches(activity, report).Should().BeTrue();
    }

    [Theory]
    [InlineData(Data.Enums.Sex.Male)]
    [InlineData(Data.Enums.Sex.Female)]
    [InlineData(Data.Enums.Sex.Unknown)]
    public void Matches_WithMatching_NoSexSpecified_ReturnsTrue(Data.Enums.Sex sex)
    {
        var report = new IndividualHuntedMortalityReport() { Season = new HuntingSeason(2023) };
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new HuntingBagLimitEntry
        {
            Areas = new() { area },
            Season = (HuntingSeason)report.Season,
            Sex = null,
            Species = Data.Enums.Species.Moose,
            PeriodStart = report.Season.StartDate,
            PeriodEnd = report.Season.EndDate
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality
            {
                Sex = sex,
                DateOfDeath = report.Season.StartDate.AddDays(20)
            },
            GameManagementArea = area,
        };

        entry.Matches(activity, report).Should().BeTrue();
    }

    [Fact]
    public void Matches_WithDifferentSex_ReturnsFalse()
    {
        var report = new IndividualHuntedMortalityReport() { Season = new HuntingSeason(2023) };
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new HuntingBagLimitEntry
        {
            Areas = new() { area },
            Season = (HuntingSeason)report.Season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose,
            PeriodStart = report.Season.StartDate,
            PeriodEnd = report.Season.EndDate
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality
            {
                Sex = Data.Enums.Sex.Female,
                DateOfDeath = report.Season.StartDate.AddDays(2)
            },
            GameManagementArea = area,
        };

        entry.Matches(activity, report).Should().BeFalse();
    }

    [Fact]
    public void Matches_WithDifferentSpecies_ReturnsFalse()
    {
        var report = new IndividualHuntedMortalityReport() { Season = new HuntingSeason(2023) };
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new HuntingBagLimitEntry
        {
            Areas = new() { area },
            Season = (HuntingSeason)report.Season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose
        };

        var activity = new HuntedActivity
        {
            Mortality = new CaribouMortality { Sex = Data.Enums.Sex.Male },
            GameManagementArea = area,
        };

        entry.Matches(activity, report).Should().BeFalse();
    }

    [Fact]
    public void Matches_WithDifferentSeason_ReturnsFalse()
    {
        var report2023 = new IndividualHuntedMortalityReport() { Season = new HuntingSeason(2023) };
        var report2024 = new IndividualHuntedMortalityReport() { Season = new HuntingSeason(2024) };

        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new HuntingBagLimitEntry
        {
            Areas = new() { area },
            Season = (HuntingSeason)report2023.Season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality { Sex = Data.Enums.Sex.Male },
            GameManagementArea = area,
        };

        entry.Matches(activity, report2024).Should().BeFalse();
    }

    [Fact]
    public void Matches_WithDifferentArea_ReturnsFalse()
    {
        var report = new IndividualHuntedMortalityReport() { Season = new HuntingSeason(2023) };

        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new HuntingBagLimitEntry
        {
            Areas = new() { area },
            Season = (HuntingSeason)report.Season,
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

        entry.Matches(activity, report).Should().BeFalse();
    }

    [Fact]
    public void Matches_WithDifferentPeriod_ReturnsFalse()
    {
        var report = new IndividualHuntedMortalityReport() { Season = new HuntingSeason(2023) };
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new HuntingBagLimitEntry
        {
            Areas = new() { area },
            Season = (HuntingSeason)report.Season,
            Sex = Data.Enums.Sex.Male,
            Species = Data.Enums.Species.Moose,
            PeriodStart = report.Season.StartDate.AddDays(1),
            PeriodEnd = report.Season.EndDate.AddDays(-1)
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

            entry.Matches(activity, report).Should().BeFalse();
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

            entry.Matches(activity, report).Should().BeFalse();
        }
    }
}
