using WildlifeMortalities.PosseExampleApi.Models;

namespace WildlifeMortalities.PosseExampleApi.Features.Authorizations.GetByEnvClientId;

public class Endpoint : Endpoint<GetAuthorizationsByEnvClientIdRequest, GetAuthorizationsByEnvClientIdResponse>
{
    public override void Configure()
    {
        Get("/authorizations/{EnvClientId}");
        Policies("ApiKey");
        Description(b => b
            .Produces<GetAuthorizationsByEnvClientIdResponse>()
            .Produces(404));
        Summary(s =>
        {
            s.ResponseExamples[200] = new GetAuthorizationsByEnvClientIdResponse
            {
                Authorizations = new List<Authorization>()
                {
                    new(
                        AuthorizationType.BigGameHuntingLicence_YukonResident,
                        "532304",
                        "EHL-24202", null, null,
                        new DateTimeOffset(2021, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        new DateTimeOffset(2021, 9, 12, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now),
                    new(
                        AuthorizationType.HuntingSeal_AmericanBlackBear,
                        "532304",
                        "EHS-2420", null, null,
                        new DateTimeOffset(2022, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        new DateTimeOffset(2023, 3, 31, 23, 59, 59, 999, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now),
                    new(
                    AuthorizationType.WildlifeActPermit,
                    "532304",
                    "EHS-2420", "You must...", null,
                    new DateTimeOffset(2022, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                    new DateTimeOffset(2023, 3, 31, 23, 59, 59, 999, new TimeSpan(-7, 0, 0)),
                    DateTimeOffset.Now),
                    new(
                        AuthorizationType.SpecialGuideLicence,
                        "532304",
                        "EHS-2420", null, "403203",
                        new DateTimeOffset(2022, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        new DateTimeOffset(2023, 3, 31, 23, 59, 59, 999, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now)
                }
            };
        });
    }

    public override Task HandleAsync(GetAuthorizationsByEnvClientIdRequest req, CancellationToken ct)
    {
        var response = new GetAuthorizationsByEnvClientIdResponse()
        {
            Authorizations = new List<Authorization>()
            {
                new(req.AuthorizationType ?? AuthorizationType.BigGameHuntingLicence_CanadianResident,
                    req.EnvClientId ?? "41203", "EHL-24202", null, null,
                    DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now)
            }
        };

        return SendAsync(response);
    }
}

public class GetAuthorizationsByEnvClientIdRequest
{
    public string? EnvClientId { get; set; }
    public AuthorizationType? AuthorizationType { get; set; }
    public DateTimeOffset? ModifiedSinceDateTime { get; set; }
}

public class GetAuthorizationsByEnvClientIdResponse
{
    public List<Authorization> Authorizations { get; set; }
}
