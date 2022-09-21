using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Licences;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;

Console.WriteLine("Starting data seeding...");
Console.WriteLine("-----------------------");
using (var context = new AppDbContext())
{
    AddAllGameManagementAreas(context);
    AddAllGameManagementAreaSpecies(context);
    AddFakeClients(context);
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
            var subzone = i < 10 ? $"0{i}" : i.ToString();
            context.GameManagementAreas.Add(
                new GameManagementArea { Zone = zone.ToString(), Subzone = subzone }
            );
        }
    }
}

void AddAllGameManagementAreaSpecies(AppDbContext context)
{
    if (!context.GameManagementAreaSpecies.Any())
    {
        foreach (
            HuntedSpeciesWithGameManagementArea species in Enum.GetValues(
                typeof(HuntedSpeciesWithGameManagementArea)
            )
        )
        {
            if (species != HuntedSpeciesWithGameManagementArea.Uninitialized)
            {
                var gameManagementAreas = context.GameManagementAreas.ToList();
                foreach (var area in gameManagementAreas)
                {
                    context.GameManagementAreaSpecies.Add(
                        new GameManagementAreaSpecies
                        {
                            Species = species,
                            GameManagementArea = area
                        }
                    );
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

void AddFakeClients(AppDbContext context)
{
    if (!context.People.OfType<Client>().Any())
    {
        var clients = new List<Client>();
        var rand = new Random();

        for (var i = 1; i < 100; i++)
        {
            clients.Add(new Client { EnvClientId = rand.Next(10000, 80000).ToString() });
        }

        foreach (var client in clients)
        {
            AddLicences(client);
        }

        void AddLicences(Client client)
        {
            for (var i = 0; i < rand.Next(0, 4); i++)
            {
                client.Licences.Add(
                    new HuntingLicence { Number = $"EHL-{rand.Next(1000, 99999)}" }
                );
            }

            foreach (var licence in client.Licences.OfType<HuntingLicence>())
            {
                licence.StartDate = new DateTime(rand.Next(2019, 2022), 04, 01);
                licence.EndDate = new DateTime(licence.StartDate.Year + 1, 03, 31);
                AddSeals(licence);
            }

            void AddSeals(HuntingLicence licence)
            {
                for (var i = 0; i < rand.Next(0, 5); i++)
                {
                    licence.Seals.Add(
                        new Seal
                        {
                            Number = $"EHS-{rand.Next(1000, 99999)}",
                            Species = HuntedSpecies.WoodBison
                        }
                    );
                }
            }
        }
        context.AddRange(clients);
        context.SaveChanges();
        Console.WriteLine("Added fake Clients");
    }
    else
    {
        Console.WriteLine("Fake Clients already exist");
    }
}
