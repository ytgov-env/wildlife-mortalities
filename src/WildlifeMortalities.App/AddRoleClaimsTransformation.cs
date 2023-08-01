using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.App;

public class AddRoleClaimsTransformation : IClaimsTransformation
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AddRoleClaimsTransformation(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var nameIdentifier =
            principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";

        var claimsIdentity = new ClaimsIdentity();
        var permissions = await _context.Users
            .Where(x => x.NameIdentifier == nameIdentifier)
            .Select(x => x.Permissions)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (permissions == null)
        {
            return principal;
        }

        foreach (var permission in permissions)
        {
            claimsIdentity.AddClaim(new Claim(claimsIdentity.RoleClaimType, permission.Value));
        }

        if (permissions.Any())
        {
            claimsIdentity.AddClaim(new Claim(claimsIdentity.RoleClaimType, "IsKnownUser"));
        }

        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}
