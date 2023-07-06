using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.App.Features.Shared;

public class DbContextAwareComponent : ComponentBase, IDisposable
{
    private bool _disposedValue;

    private ReadOnlyAppDbContext? _context;

    // Using an in memory database if _context is null is a workaround to avoid exceptions that terminate the circuit
    // when a user navigates away from a page while async lifecycle methods are still running. The framework can call
    // dispose at any time.
    protected ReadOnlyAppDbContext Context =>
        _context
        ?? new ReadOnlyAppDbContext(
            new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("unused").Options
        );

    [Inject]
    private IDbContextFactory<ReadOnlyAppDbContext> DbContextFactory { get; set; } = null!;

    protected override void OnInitialized()
    {
        _context = DbContextFactory.CreateDbContext();
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _context?.Dispose();
                _context = null;
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
