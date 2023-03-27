namespace WildlifeMortalities.Data;

public class ReadOnlyAppDbContext : AppDbContext
{
    public override int SaveChanges()
    {
        throw new InvalidOperationException();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new InvalidOperationException();
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        throw new InvalidOperationException();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException();
    }
}
