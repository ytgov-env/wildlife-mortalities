namespace WildlifeMortalities.Data;

public class ReadOnlyAppDbContext : AppDbContext
{
    private const string ExceptionMessage =
        $"This dbContext is read-only. All mutations should be performed using {nameof(AppDbContext)}.";

    public override int SaveChanges()
    {
        throw new InvalidOperationException(ExceptionMessage);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new InvalidOperationException(ExceptionMessage);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        throw new InvalidOperationException(ExceptionMessage);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException(ExceptionMessage);
    }
}
