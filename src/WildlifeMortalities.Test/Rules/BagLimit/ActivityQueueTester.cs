using System;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using Xunit;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using System.Collections;

namespace WildlifeMortalities.Test.Rules.BagLimit;

public class ActivityQueueTester
{
    [Fact]
    public void AddToQueue_EmptyQueueWithSingleActivity()
    {
        var bagLimitEntry = new BagLimitEntry();
    }

    private class ActivityQueueComparer : IComparer<HuntedActivity>
    {
        public int Compare(HuntedActivity? x, HuntedActivity? y)
        {
            if (x.Mortality.DateOfDeath > y.Mortality.DateOfDeath)
            {
                return 1;
            }
            else if (x.Mortality.DateOfDeath < y.Mortality.DateOfDeath)
            {
                return -1;
            }
            else
            {
                return x.CreatedTimestamp >= y.CreatedTimestamp ? 1 : -1;
            }
        }
    }

    [Fact]
    public async Task AddToQueue_ReorderByDateOfDeath_OrderedByDateOfDeath()
    {
        const int MaxEntries = 12;
        const int EntriesToRemove = 5;

        var season = new HuntingSeason(2023);
        var area = new GameManagementArea
        {
            Zone = "4",
            Subzone = "03",
            Id = 10,
        };

        var bagLimitEntry = new CaribouBagLimitEntry
        {
            Areas = new() { area },
            Herds = new() { CaribouMortality.CaribouHerd.Atlin },
            MaxValuePerPerson = 1,
            Season = season,
            PeriodStart = season.StartDate,
            PeriodEnd = season.EndDate,
            ActivityQueue = new(),
            MaxValueForThreshold = 5
        };

        Random random = new(13245);

        var start = DateTimeOffset.Now.Date.AddDays(-7);
        var end = DateTimeOffset.Now.Date;

        var daySpan = (int)(end - start).TotalDays;

        var mapper = new List<HuntedActivity>();

        for (var i = 0; i < MaxEntries; i++)
        {
            var dateOfDeath = start.AddDays(random.Next(0, daySpan)).Date;
            var activity = new HuntedActivity()
            {
                Id = i,
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(dateOfDeath, TimeSpan.FromHours(-7)),
                    Herd = CaribouMortality.CaribouHerd.Atlin,
                    Sex = Data.Enums.Sex.Male
                },
                GameManagementArea = area,
                CreatedTimestamp = DateTimeOffset.Now,
            };
            bagLimitEntry.AddToQueue(activity);

            await Task.Delay(10);

            mapper.Add(activity);
        }

        mapper.Sort(new ActivityQueueComparer());

        bagLimitEntry.ActivityQueue.Should().HaveCount(MaxEntries);
        bagLimitEntry.ActivityQueue
            .Select(x => x.Position)
            .Distinct()
            .Should()
            .BeEquivalentTo(Enumerable.Range(1, MaxEntries));

        foreach (var item in bagLimitEntry.ActivityQueue)
        {
            item.Activity.Should().Be(mapper[item.Position - 1]);
        }

        for (var i = 0; i < EntriesToRemove; i++)
        {
            var activity = mapper[random.Next(0, mapper.Count)];
            bagLimitEntry.RemoveFromQueue(activity);
            mapper.Remove(activity);
        }

        mapper.Sort(new ActivityQueueComparer());
        bagLimitEntry.ActivityQueue.Should().HaveCount(MaxEntries - EntriesToRemove);
        bagLimitEntry.ActivityQueue
            .Select(x => x.Position)
            .Distinct()
            .Should()
            .BeEquivalentTo(Enumerable.Range(1, MaxEntries - EntriesToRemove));

        foreach (var item in bagLimitEntry.ActivityQueue)
        {
            item.Activity.Should().Be(mapper[item.Position - 1]);
        }
    }
}
