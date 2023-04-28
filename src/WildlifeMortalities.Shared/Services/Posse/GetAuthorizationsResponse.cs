// ReSharper disable InconsistentNaming

namespace WildlifeMortalities.Shared.Services.Posse;

public class GetAuthorizationsResponse
{
    public IEnumerable<AuthorizationDto> Authorizations { get; set; } = null!;
}
