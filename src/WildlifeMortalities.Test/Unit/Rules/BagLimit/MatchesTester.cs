using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Test.Unit.Rules.BagLimit;

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
        var entry = new HuntingBagLimitEntry(
            new[] { area },
            Species.Moose,
            (HuntingSeason)report.Season,
            report.Season.StartDate,
            report.Season.EndDate,
            int.MaxValue,
            Sex.Male
        );

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality
            {
                Sex = Sex.Male,
                DateOfDeath = report.Season.StartDate.AddDays(2)
            },
            GameManagementArea = area,
        };

        entry.Matches(activity, report).Should().BeTrue();
    }

    [Theory]
    [InlineData(Sex.Male)]
    [InlineData(Sex.Female)]
    [InlineData(Sex.Unknown)]
    public void Matches_WithMatching_NoSexSpecified_ReturnsTrue(Sex sex)
    {
        var report = new IndividualHuntedMortalityReport() { Season = new HuntingSeason(2023) };
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };
        var entry = new HuntingBagLimitEntry(
            new[] { area },
            Species.Moose,
            (HuntingSeason)report.Season,
            report.Season.StartDate,
            report.Season.EndDate,
            int.MaxValue
        );

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
        var entry = new HuntingBagLimitEntry(
            new[] { area },
            Species.Moose,
            (HuntingSeason)report.Season,
            report.Season.StartDate,
            report.Season.EndDate,
            int.MaxValue,
            Sex.Male
        )
        {
            Areas = new() { area },
            Season = (HuntingSeason)report.Season,
            Sex = Sex.Male,
            Species = Species.Moose,
            PeriodStart = report.Season.StartDate,
            PeriodEnd = report.Season.EndDate
        };

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality
            {
                Sex = Sex.Female,
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
        var entry = new HuntingBagLimitEntry(
            new[] { area },
            Species.Moose,
            (HuntingSeason)report.Season,
            report.Season.StartDate,
            report.Season.EndDate,
            int.MaxValue,
            Sex.Male
        );

        var activity = new HuntedActivity
        {
            Mortality = new CaribouMortality { Sex = Sex.Male },
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
        var entry = new HuntingBagLimitEntry(
            new[] { area },
            Species.Moose,
            (HuntingSeason)report2023.Season,
            report2023.Season.StartDate,
            report2023.Season.EndDate,
            int.MaxValue,
            Sex.Male
        );

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality { Sex = Sex.Male },
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
        var entry = new HuntingBagLimitEntry(
            new[] { area },
            Species.Moose,
            (HuntingSeason)report.Season,
            report.Season.StartDate,
            report.Season.EndDate,
            int.MaxValue,
            Sex.Male
        );

        var activity = new HuntedActivity
        {
            Mortality = new MooseMortality { Sex = Sex.Male },
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
        var entry = new HuntingBagLimitEntry(
            new[] { area },
            Species.Moose,
            (HuntingSeason)report.Season,
            report.Season.StartDate.AddDays(1),
            report.Season.EndDate.AddDays(-1),
            int.MaxValue,
            Sex.Male
        );

        {
            var activity = new HuntedActivity
            {
                Mortality = new MooseMortality
                {
                    Sex = Sex.Male,
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
                    Sex = Sex.Male,
                    DateOfDeath = entry.PeriodEnd.AddDays(1)
                },
                GameManagementArea = area,
            };

            entry.Matches(activity, report).Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(TrappedActivity.HarvestMethodType.NeckSnare, true)]
    [InlineData(TrappedActivity.HarvestMethodType.ConibearTrap, false)]
    [InlineData(TrappedActivity.HarvestMethodType.LegholdTrap, false)]
    [InlineData(TrappedActivity.HarvestMethodType.Other, false)]
    public void Matches_WithMatchingGreyWolf_ReturnsTrue(
        TrappedActivity.HarvestMethodType type,
        bool shouldMatch
    )
    {
        var concession = new RegisteredTrappingConcession { Id = 10, Concession = "09" };
        var report = new TrappedMortalitiesReport()
        {
            Season = new TrappingSeason(2023),
            RegisteredTrappingConcession = concession
        };

        var entry = new NeckSnaredGreyWolfTrappingBagLimitEntry(
            new[] { concession },
            (TrappingSeason)report.Season,
            report.Season.StartDate,
            report.Season.EndDate,
            int.MaxValue,
            Sex.Male
        );

        var activity = new TrappedActivity
        {
            Mortality = new GreyWolfMortality
            {
                Sex = Sex.Male,
                DateOfDeath = report.Season.StartDate.AddDays(2)
            },
            HarvestMethod = type
        };

        entry.Matches(activity, report).Should().Be(shouldMatch);
    }
}
