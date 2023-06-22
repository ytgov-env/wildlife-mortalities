using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.Test.Helpers;

public class TestDbContextFactory : IDbContextFactory<AppDbContext>
{
    private readonly DbContextOptions<AppDbContext> _options;

    public TestDbContextFactory(DbContextOptions<AppDbContext> options, bool cleanRequired = true)
    {
        _options = options;
        if (cleanRequired)
        {
            using var context = CreateDbContext();
            context.Database.EnsureClean();
        }
    }

    public AppDbContext CreateDbContext() => new(_options);
}
