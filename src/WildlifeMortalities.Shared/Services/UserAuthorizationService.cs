using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.Shared.Services;

public class UserAuthorizationService : IUserAuthorizationService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public UserAuthorizationService(IDbContextFactory<AppDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public async Task UpdatePermission(int userId, string permission, bool hasPermission)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var user =
            await context.Users.Include(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new ArgumentException($"User with id {userId} does not exist.");

        var permissionEntry = context.Permissions.FirstOrDefault(x => x.Value == permission);
        if (permissionEntry == null)
        {
            permissionEntry = new Permission { Value = permission };
            context.Permissions.Add(permissionEntry);
        }

        var userHasPermission = user.Permissions.Any(x => x.Value == permission);

        if (hasPermission)
        {
            if (!userHasPermission)
            {
                user.Permissions.Add(permissionEntry);
            }
        }
        else
        {
            user.Permissions.Remove(permissionEntry);
        }

        await context.SaveChangesAsync();
    }

    public async Task<bool> CreateUser(string email)
    {
        using var context = _dbContextFactory.CreateDbContext();

        if (await context.Users.AnyAsync(x => x.EmailAddress == email))
        {
            return false;
        }

        var user = new User()
        {
            EmailAddress = email,
            Settings = new() { IsDarkMode = false }
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUser(int userId)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var user =
            await context.Users
                .AsSplitQuery()
                .Include(x => x.CreatedReports)
                .Include(x => x.ModifiedReports)
                .Include(x => x.CreatedDraftReports)
                .Include(x => x.ModifiedDraftReports)
                .SingleOrDefaultAsync(x => x.Id == userId)
            ?? throw new ArgumentException($"User with id {userId} does not exist.");

        if (
            !user.CreatedReports.Any()
            && !user.ModifiedReports.Any()
            && !user.CreatedDraftReports.Any()
            && !user.ModifiedDraftReports.Any()
        )
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
