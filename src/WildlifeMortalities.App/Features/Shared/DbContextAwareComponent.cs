using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.App.Features.Shared;

public class DbContextAwareComponent : ComponentBase
{
    [Inject]
    private IDbContextFactory<ReadOnlyAppDbContext> DbContextFactory { get; set; } = null!;

    protected ReadOnlyAppDbContext GetContext()
    {
        var context = DbContextFactory.CreateDbContext();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        return context;
    }
}
