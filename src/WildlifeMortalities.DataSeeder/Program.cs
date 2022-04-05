using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Enums;

Console.WriteLine("Starting data seeding...");
Console.WriteLine("-----------------------");

using (var context = new AppDbContext())
{
    AddAllGameManagementAreas(context);
    AddAllGameManagementAreaSpecies(context);
}

Console.WriteLine("---------------------");
Console.WriteLine("Data seeding complete");

void AddAllGameManagementAreas(AppDbContext context)
{
    if (!context.GameManagementAreas.Any())
    {
        // 101-172
        AddGameManagementAreas(context, 1, 72);
        // 201-293
        AddGameManagementAreas(context, 2, 93);
        // 301-320
        AddGameManagementAreas(context, 3, 20);
        // 401-452
        AddGameManagementAreas(context, 4, 52);
        // 501-551
        AddGameManagementAreas(context, 5, 51);
        // 601-613
        AddGameManagementAreas(context, 6, 13);
        // 701-736
        AddGameManagementAreas(context, 7, 36);
        // 801-827
        AddGameManagementAreas(context, 8, 27);
        // 901-911
        AddGameManagementAreas(context, 9, 11);
        // 1001-1032
        AddGameManagementAreas(context, 10, 32);
        // 1101-1146
        AddGameManagementAreas(context, 11, 46);
        context.SaveChanges();
        Console.WriteLine("Added GameManagementAreas");
    }
    else
    {
        Console.WriteLine("GameManagementAreas already exist");
    }

    static void AddGameManagementAreas(AppDbContext context, int zone, int maxSubzone)
    {
        for (var i = 1; i <= maxSubzone; i++)
        {
            context.GameManagementAreas.Add(new GameManagementArea { Zone = zone, Subzone = i });
        }
    }
}

void AddAllGameManagementAreaSpecies(AppDbContext context)
{
    if (!context.GameManagementAreaSpecies.Any())
    {
        foreach (GmaSpecies species in Enum.GetValues(typeof(GmaSpecies)))
        {
            if (species != GmaSpecies.Uninitialized)
            {
                var gameManagementAreas = context.GameManagementAreas.ToList();
                foreach (var area in gameManagementAreas)
                {
                    context.GameManagementAreaSpecies.Add(new GameManagementAreaSpecies { Species = species, GameManagementArea = area });
                }
            }
        }
        context.SaveChanges();
        Console.WriteLine("Added GameManagementAreaSpecies");
    }
    else
    {
        Console.WriteLine("GameManagementAreaSpecies already exist");
    }
}