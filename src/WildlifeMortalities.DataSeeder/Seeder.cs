using System.Text.Json;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Seasons;
using Constants = WildlifeMortalities.Shared.Constants;

namespace WildlifeMortalities.DataSeeder;

public static class Seeder
{
    public static async Task Seed(AppDbContext context)
    {
        AddPosseSyncKeys(context);
        AddAllSeasons(context);
        AreaSeeder.AddAllGameManagementAreas(context);
        AreaSeeder.AddAllOutfitterAreas(context);
        AreaSeeder.AddAllRegisteredTrappingConcessions(context);
        await new BagLimitSeeder(context).AddAllBagLimitEntries();
    }

    private static void AddAllSeasons(AppDbContext context)
    {
        if (!context.Seasons.Any())
        {
            for (var i = 2000; i < 2100; i++)
            {
                context.Seasons.Add(new HuntingSeason(i));
                context.Seasons.Add(new TrappingSeason(i));
                context.Seasons.Add(new CalendarSeason(i));
            }

            context.SaveChanges();
            Console.WriteLine("Added Seasons");
        }
        else
        {
            Console.WriteLine("Seasons already exist");
        }
    }

    private static void AddPosseSyncKeys(AppDbContext context)
    {
        if (!context.AppConfigurations.Any())
        {
            context.AppConfigurations.AddRange(
                new AppConfiguration()
                {
                    Key = Constants.AppConfigurationService.LastSuccessfulClientsSyncKey,
                    Value = JsonSerializer.Serialize(new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero))
                },
                new AppConfiguration()
                {
                    Key = Constants.AppConfigurationService.LastSuccessfulAuthorizationsSyncKey,
                    Value = JsonSerializer.Serialize(new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero))
                });
            context.SaveChanges();
            Console.WriteLine("Added posse sync k/v pairs");
        }
        else
        {
            Console.WriteLine("Posse sync k/v pairs already exist");
        }
    }
}
