using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;

var context = new AppDbContext();

var bisonMortality = new Mortality() { HarvestReport = new HarvestReport() { DateKilled = DateTime.Now }, Animal = new Bison() { Pregnant = true, Wounded = false } };

context.Add(bisonMortality);

var birdMortality = new Mortality() { HarvestReport = new HarvestReport() { DateKilled = DateTime.Now }, Animal = new Bird() { Quantity = 4 } };

context.Add(birdMortality);

context.SaveChanges();

var mortalities = context.Mortalities.Include(m => m.Animal);
Console.WriteLine(typeof(Bird).Name);

var birdMortalities = context.Mortalities.Where(m => m.Animal.Species == typeof(Bird).Name).Include(m => m.Animal);

Console.WriteLine("blah");