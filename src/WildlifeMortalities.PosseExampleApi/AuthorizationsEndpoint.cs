using WildlifeMortalities.PosseExampleApi.Models;
using YamlDotNet.Core.Tokens;

namespace WildlifeMortalities.PosseExampleApi;

public class AuthorizationsEndpoint : Endpoint<AuthorizationsRequest, AuthorizationsResponse>
{
    public override void Configure()
    {
        Get("/authorizations");
        Policies("ApiKey");
        Description(b => b
            .Produces<List<AuthorizationDto>>()
            .Produces(404));
    }

    public override Task HandleAsync(AuthorizationsRequest req, CancellationToken ct)
    {
        var response = new AuthorizationsResponse()
        {
            Authorizations = new List<AuthorizationDto>()
            {
                new(req.AuthorizationType ?? AuthorizationType.BigGameHuntingLicence, req.EnvClientId ?? "41203", "EHL-24202",
                    DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now)
            }
        };

        return SendAsync(response);
    }
}

public class AuthorizationsRequest
{
    public string? EnvClientId { get; set; }
    public AuthorizationType? AuthorizationType { get; set; }
    public DateTimeOffset? ModifiedSinceDateTime { get; set; }
}

public class AuthorizationsResponse
{
    public List<AuthorizationDto> Authorizations { get; set; }
}
