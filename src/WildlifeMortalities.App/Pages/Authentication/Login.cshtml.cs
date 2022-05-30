using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WildlifeMortalities.App.Pages.Authentication
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private IConfiguration Configuration { get; set; }

        public LoginModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task OnGet(string redirectUri)
        {
            await HttpContext.ChallengeAsync(
                Configuration["AuthNProvider:Name"],
                new AuthenticationProperties { RedirectUri = redirectUri }
            );
        }
    }
}
