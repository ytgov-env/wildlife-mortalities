using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.App.Features.Shared;

public class DbContextAwareComponent : ComponentBase, IDisposable
{
    private bool _disposedValue;

    protected ReadOnlyAppDbContext Context { get; private set; } = null!;

    [Inject]
    private IDbContextFactory<ReadOnlyAppDbContext> DbContextFactory { get; set; } = null!;

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
        GC.SuppressFinalize(this);
    }
}
