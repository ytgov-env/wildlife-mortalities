using System.Security.Claims;

namespace WildlifeMortalities.App.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email);
        return email != null ? email.Value : string.Empty;
    }

    public static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
    {
        var firstName = claimsPrincipal.FindFirst(ClaimTypes.GivenName);
        return firstName != null ? firstName.Value : string.Empty;
    }

    public static string GetLastName(this ClaimsPrincipal claimsPrincipal)
    {
        var lastName = claimsPrincipal.FindFirst(ClaimTypes.Surname);
        return lastName != null ? lastName.Value : string.Empty;
    }

    public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
    {
        return $"{claimsPrincipal.GetFirstName()} {claimsPrincipal.GetLastName()}";
    }
}
