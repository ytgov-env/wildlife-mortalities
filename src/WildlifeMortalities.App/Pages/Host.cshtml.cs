using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.App.Pages
{
    public class HostModel : PageModel
    {
        private readonly AppDbContext _context;

        public AppParameters AppParameters { get; set; } = new AppParameters();

        public HostModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return Page();
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
            {
                return Page();
            }

            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nameIdentifier))
            {
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.NameIdentifier == nameIdentifier
            );
            if (user == null)
            {
                user = new User
                {
                    NameIdentifier = nameIdentifier,
                    Name = GetName(email),
                    EmailAddress = email,
                    Settings = UserSettings.Default
                };

                _context.Users.Add(user);
            }
            else
            {
                user.Name = GetName(email);
                user.EmailAddress = email;
            }
            await _context.SaveChangesAsync();

            AppParameters = new AppParameters
            {
                UserId = user.Id,
                UserEmail = email,
                UserSettings = user.Settings,
            };

            Log.Information("User {Email}, {NameIdentifier} logged in", email, nameIdentifier);

            return Page();

            static string GetName(string email) => email[..email.IndexOf('@')].Replace('.', ' ');
        }
    }
}
