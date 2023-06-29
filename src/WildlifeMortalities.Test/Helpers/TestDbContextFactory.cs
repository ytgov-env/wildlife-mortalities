using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.Test.Helpers;

public static class TestDbContextFactory
{
    public static AppDbContext GetContext()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        var contextName = ThreadSafeRandom.Next().ToString();
        builder.UseInMemoryDatabase(contextName);

        return new AppDbContext(builder.Options);
    }
}
