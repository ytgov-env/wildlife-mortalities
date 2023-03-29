using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.App
{
    public class AppParameters
    {
        public IDbContextFactory<AppDbContext> ContextFactory { get; set; } = null!;

        public UserSettings UserSettings { get; set; } = new();
        public string UserEmail { get; set; } = string.Empty;
        public int UserId { get; set; }

        public async Task UpdateSettings()
        {
            using var context = ContextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(UserId);
            if (user != null)
            {
                user.Settings = UserSettings;

                await context.SaveChangesAsync();
            }
        }
    }
}
