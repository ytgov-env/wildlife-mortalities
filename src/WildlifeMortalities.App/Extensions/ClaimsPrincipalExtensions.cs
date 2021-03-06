using System.Security.Claims;

namespace WildlifeMortalities.App.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetFormattedName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value.Replace('.', ' ') ?? "";
    }
}
