using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.Data;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EnvWildlifeMortalities;Integrated Security=True;"
    )
    .EnableSensitiveDataLogging()
    .Options;

using var setupContext = new AppDbContext(options);

//var reports = await setupContext.Reports.WithEntireGraph().AsNoTracking().ToListAsync();
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

//foreach (var report in reports)
//{
//    using var context = new AppDbContext(options);
//    var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
//    contextFactory.Setup(contextFactory => contextFactory.CreateDbContext()).Returns(context);
//    var service = new MortalityService(contextFactory.Object);
//    await service.UpdateReport(report, 1);
//}
