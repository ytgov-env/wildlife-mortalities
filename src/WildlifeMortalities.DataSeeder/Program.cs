using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.DataSeeder;

Console.WriteLine("Starting data seeding...");
Console.WriteLine("-----------------------");
using (var context = new AppDbContext())
{
    AddAllSeasons(context);
    AreaSeeder.AddAllGameManagementAreas(context);
    AreaSeeder.AddAllOutfitterAreas(context);
    AreaSeeder.AddAllRegisteredTrappingConcessions(context);
    await new BagLimitSeeder(context).AddAllBagLimitEntries();
}

Console.WriteLine("---------------------");
Console.WriteLine("Data seeding complete");

static void AddAllSeasons(AppDbContext context)
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
