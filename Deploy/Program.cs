using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Moq;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Services;
using WildlifeMortalities.Shared.Services.Reports.Single;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EnvWildlifeMortalities;Integrated Security=True;"
    )
    .EnableSensitiveDataLogging()
    .Options;

using var setupContext = new AppDbContext(options);

var users = await setupContext.Users.Include(x => x.Permissions).ToListAsync();

var permissions = PermissionsHelper
    .GetPermissions()
    .SelectMany(x => x.Permissions)
    .Select(x => new WildlifeMortalities.Data.Entities.Users.Permission { Value = x.Value })
    .ToList();

foreach (var item in permissions)
{
    setupContext.Permissions.AddRange(permissions);
}

foreach (var user in users)
{
    user.Permissions.AddRange(permissions);
}

await setupContext.SaveChangesAsync();

var reports = await setupContext.Reports.WithEntireGraph().AsNoTracking().ToListAsync();

//var temp1 = await setupContext.Activities
//    .OfType<HarvestActivity>()
//    .Where(x => x.ActivityQueueItem == null || x.ActivityQueueItem.BagLimitEntry == null)
//    .ToListAsync();

//var bagEntries = await setupContext.BagEntries
//    .Where(x => x.SharedValue == 0)
//    .SumAsync(x => x.CurrentValue);

//var bagLimitEntries = await setupContext.BagLimitEntries
//    .Include(x => x.MaxValuePerPersonSharedWith)
//    .Include(x => x.ActivityQueue)
//    .ThenInclude(x => x.Activity)
//    .ThenInclude(x => x.Mortality)
//    .Include(x => ((HuntingBagLimitEntry)x).Areas)
//    .Include(x => ((HuntingBagLimitEntry)x).Season)
//    .Include(x => ((TrappingBagLimitEntry)x).Concessions)
//    .AsSplitQuery()
//    .ToArrayAsync();

//Dictionary<int, List<(int baglimitEntryId, Species species)>> counters = reports
//    .Select(x => x.GetPerson().Id)
//    .Distinct()
//    .ToDictionary(x => x, x => new List<(int, Species)>());

//foreach (var item in reports)
//{
//    foreach (var activity in item.GetActivities().OfType<HarvestActivity>())
//    {
//        var match = bagLimitEntries.FirstOrDefault(x => x.Matches(activity, item));
//        if (match == null)
//        {
//            throw new Exception();
//        }

//        counters[item.GetPerson().Id].Add((match.Id, activity.Mortality.Species));
//    }
//}

//foreach (var item in counters)
//{
//    var groupedSpecies = item.Value.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

//    if (groupedSpecies.Any(x => x.Value > 1))
//    {
//        var a = 4;
//    }
//}

foreach (var report in reports)
{
    using var context = new AppDbContext(options);
    var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
    contextFactory.Setup(contextFactory => contextFactory.CreateDbContext()).Returns(context);
    var service = new MortalityService(contextFactory.Object);
    await service.UpdateReport(report, 1);
}

Console.ReadLine();
