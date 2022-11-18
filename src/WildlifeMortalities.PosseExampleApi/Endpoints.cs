using Ardalis.Result;
using WildlifeMortalities.PosseExampleApi.Models;
using WildlifeMortalities.Shared.Services;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WildlifeMortalities.PosseExampleApi;

public static class Endpoints
{
    private const string Tag ="Clients";
    private const string BaseRoute = "api/v1/clients";
    public static void MapClientEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/clients/{envClientId}/authorizations", GetAuthorizationsByEnvClientId)
            .WithTags("Authorizations")
            .WithName("GetAuthorizations")
            .Produces<List<AuthorizationDto>>()
            .Produces(404);

        app.MapGet("api/v1/clients/{envClientId}", GetClientByEnvClientId)
            .WithTags("Clients")
            .WithName("GetClient")
            .Produces<ClientDto>()
            .Produces(404);
    }

    private static async Task<IResult> GetAuthorizationsByEnvClientId(string envClientId)
    {
        var result = new List<AuthorizationDto>();
        if (envClientId == "111")
        {
            return Results.NotFound();
        }

        result.Add(new AuthorizationDto(AuthorizationType.BigGameHuntingLicence, envClientId, "EHL-3123",
            DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(1), DateTime.Now
        ));

        return Results.Ok(result);
    }

    private static async Task<IResult> GetClientByEnvClientId(string envClientId)
    {
        var result = new List<ClientDto>();
        if (envClientId == "111")
        {
            return Results.NotFound();
        }

        result.Add(new ClientDto(envClientId, "Peter", "Piper", new DateOnly(1995, 6, 12), DateTime.Now));

        return Results.Ok(result);
    }
}
