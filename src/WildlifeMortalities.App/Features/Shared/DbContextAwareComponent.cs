using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.App.Features.Shared;

public class DbContextAwareComponent : ComponentBase
{
    private bool _disposedValue;

    protected AppDbContext Context { get; private set; } = null!;

    [Inject]
#pragma warning disable RCS1170 // Use read-only auto-implemented property.
    private IDbContextFactory<AppDbContext> DbContextFactory { get; set; } = null!;
#pragma warning restore RCS1170 // Use read-only auto-implemented property.

    protected override void OnInitialized()
    {
        Context = DbContextFactory.CreateDbContext();
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
