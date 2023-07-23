using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Seasons;

namespace WildlifeMortalities.DataSeeder;
public static class Seeder
{
    public static async Task Seed(AppDbContext context)
    {
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
}
