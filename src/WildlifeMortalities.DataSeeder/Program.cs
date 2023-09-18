using WildlifeMortalities.Data;
using WildlifeMortalities.DataSeeder;

Console.WriteLine("Starting data seeding...");
Console.WriteLine("-----------------------");
var version = DataSeederVersion.None;
using var context = new AppDbContext();
await Seeder.Seed(context, version);
Console.WriteLine("---------------------");
Console.WriteLine("Data seeding complete");
