using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.App.Features.Shared;

public class DbContextAwareComponent : ComponentBase, IDisposable
{
    private bool _disposedValue;

    protected ReadOnlyAppDbContext Context { get; private set; } = null!;

    [Inject]
#pragma warning disable RCS1170 // Use read-only auto-implemented property.
    private IDbContextFactory<ReadOnlyAppDbContext> DbContextFactory { get; set; } = null!;
#pragma warning restore RCS1170 // Use read-only auto-implemented property.

    protected override void OnInitialized()
    {
        Context = DbContextFactory.CreateDbContext();
        Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Context?.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }
}
