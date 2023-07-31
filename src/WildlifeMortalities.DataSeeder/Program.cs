using WildlifeMortalities.Data;
using WildlifeMortalities.DataSeeder;

Console.WriteLine("Starting data seeding...");
Console.WriteLine("-----------------------");
using var context = new AppDbContext();
await Seeder.Seed(context);
Console.WriteLine("---------------------");
Console.WriteLine("Data seeding complete");