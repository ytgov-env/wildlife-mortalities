using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WildlifeMortalities.App.Features.Auth;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/login",
            (IConfiguration configuration, [FromQuery] string? redirectUri) =>
                Results.Challenge(
                    new AuthenticationProperties { RedirectUri = redirectUri ?? "/" },
                    new[] { configuration["AuthNProvider:Name"]! }
                )
        );
        app.MapGet(
            "/logout",
            (IConfiguration configuration) =>
                Results.SignOut(
                    new AuthenticationProperties
                    {
                        RedirectUri = configuration["AuthNProvider:SignedOutCallbackPath"]
                    },
                    new[]
                    {
                        configuration["AuthNProvider:Name"]!,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    }
                )
        );
    }
}
