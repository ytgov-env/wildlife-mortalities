using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.Test;

public class TestDbContextFactory : IDbContextFactory<AppDbContext>
{
    //private DbContextOptions<AppDbContext> _options;

    public TestDbContextFactory(string databaseName = "Test")
    {
        //_options = new DbContextOptionsBuilder<AppDbContext>()
        //    .UseInMemoryDatabase(databaseName)
        //    .Options;
    }

    public AppDbContext CreateDbContext()
    {
        return new AppDbContext();
    }
}
