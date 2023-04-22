using System.Security.Claims;

namespace WildlifeMortalities.App.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetFormattedName(this ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email);
        return email == null
            ? string.Empty
            : email.Value[..email.Value.IndexOf('@')].Replace('.', ' ');
    }
}
