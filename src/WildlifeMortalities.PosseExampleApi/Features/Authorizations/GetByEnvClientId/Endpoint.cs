using WildlifeMortalities.PosseExampleApi.Models;

namespace WildlifeMortalities.PosseExampleApi.Features.Authorizations.GetByEnvClientId;

public class Endpoint
    : Endpoint<GetAuthorizationsByEnvClientIdRequest, GetAuthorizationsByEnvClientIdResponse>
{
    public override void Configure()
    {
        Get("/authorizations/{EnvClientId}");
        Policies("ApiKey");
        Description(
            b => b.Produces<GetAuthorizationsByEnvClientIdResponse>().Produces(404).Produces(301)
        );
        Summary(s =>
        {
            s.ResponseExamples[200] = new GetAuthorizationsByEnvClientIdResponse
            {
                Authorizations = new List<Authorization>
                {
                    new(
                        AuthorizationType.BigGameHuntingLicence_YukonResident,
                        "532304",
                        "EHL-24202",
                        null,
                        null,
                        null,
                        Array.Empty<string>(),
                        null,
                        new DateTimeOffset(2021, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        new DateTimeOffset(2021, 9, 12, 23, 59, 59, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now
                    ),
                    new(
                        AuthorizationType.HuntingSeal_AmericanBlackBear,
                        "532304",
                        "EHS-2420",
                        null,
                        null,
                        null,
                        Array.Empty<string>(),
                        null,
                        new DateTimeOffset(2022, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        new DateTimeOffset(2023, 3, 31, 23, 59, 59, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now
                    ),
                    new(
                        AuthorizationType.CustomWildlifeActPermit,
                        "532304",
                        "EHS-2420",
                        "You must...",
                        null,
                        null,
                        Array.Empty<string>(),
                        null,
                        new DateTimeOffset(2022, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        new DateTimeOffset(2023, 3, 31, 23, 59, 59, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now
                    ),
                    new(
                        AuthorizationType.SpecialGuideLicence,
                        "532304",
                        "EHS-2420",
                        null,
                        "403203",
                        null,
                        Array.Empty<string>(),
                        null,
                        new DateTimeOffset(2022, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        new DateTimeOffset(2023, 3, 31, 23, 59, 59, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now
                    )
                }
            };
            s.Responses[301] =
                "Permanent Redirect. This EnvClientId has been merged. The URI for the new EnvClientId should be returned as per https://httpwg.org/specs/rfc9110.html#status.301";
        });
    }

    public override Task HandleAsync(
        GetAuthorizationsByEnvClientIdRequest req,
        CancellationToken ct
    )
    {
        var response = new GetAuthorizationsByEnvClientIdResponse
        {
            Authorizations = new List<Authorization>
            {
                new(
                    req.AuthorizationType
                    ?? AuthorizationType.BigGameHuntingLicence_CanadianResident,
                    req.EnvClientId ?? "41203",
                    "EHL-24202",
                    null,
                    null,
                    null,
                    Array.Empty<string>(),
                    null,
                    DateTimeOffset.Now,
                    DateTimeOffset.Now,
                    DateTimeOffset.Now
                )
            }
        };

        return SendAsync(response);
    }
}

public class GetAuthorizationsByEnvClientIdRequest
{
    public string? EnvClientId { get; set; }

    /// <summary>
    ///     Return all authorizations that match this type. If null, return all authorization types. The posse API does not
    ///     need to use these exact names for the authorizations - it could use the names currently used in POSSE.
    ///     Other authorization types can be added in the future, but the type names must be immutable.
    /// </summary>
    public AuthorizationType? AuthorizationType { get; set; }

    /// <summary>
    ///     Return all authorizations whose lastModifiedDateTime property is >= this datetime. If null, return all
    ///     authorizations.
    /// </summary>
    public DateTimeOffset? ModifiedSinceDateTime { get; set; }
}

public class GetAuthorizationsByEnvClientIdResponse
{
    public List<Authorization> Authorizations { get; set; }
}
