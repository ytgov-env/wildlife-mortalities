using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.Test.Integration;

public class TestDatabaseFixture
{
    private const string ConnectionString =
        //@"Server=(localdb)\mssqllocaldb;Database=WildlifeMortalitiesTest;Trusted_Connection=True";
        @"Server=localhost;Database=WildlifeMortalitiesTest;Trusted_Connection=True;TrustServerCertificate=true;";

    private static readonly object s_lock = new();
    private static bool s_databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (s_lock)
        {
            if (!s_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }

                s_databaseInitialized = true;
            }
        }
    }

    public AppDbContext CreateContext()
    {
        var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(ConnectionString, options => options.EnableRetryOnFailure())
                .Options
        );
        context.Database.BeginTransaction();
        return context;
    }
}
