using WildlifeMortalities.PosseExampleApi.Models;

namespace WildlifeMortalities.PosseExampleApi.Features.Authorizations.GetAll;

public class Endpoint : Endpoint<GetAuthorizationsRequest, GetAuthorizationsResponse>
{
    public override void Configure()
    {
        Get("/authorizations");
        Policies("ApiKey");
        Description(b => b.Produces<GetAuthorizationsResponse>());
        Summary(s =>
        {
            s.ResponseExamples[200] = new GetAuthorizationsResponse
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
                        new DateTimeOffset(2021, 9, 12, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now
                    ),
                    new(
                        AuthorizationType.PhaHuntingPermit_Elk,
                        "231030",
                        "EHP-24240",
                        null,
                        null,
                        "EL15",
                        Array.Empty<string>(),
                        null,
                        new DateTimeOffset(2022, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        new DateTimeOffset(2023, 3, 31, 23, 59, 59, 999, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now
                    ),
                    new(
                        AuthorizationType.PhaHuntingPermit_ThinhornSheep,
                        "632300",
                        "EHP-32304",
                        null,
                        null,
                        "SH105",
                        Array.Empty<string>(),
                        null,
                        new DateTimeOffset(2022, 4, 1, 0, 0, 0, new TimeSpan(-7, 0, 0)),
                        new DateTimeOffset(2023, 3, 31, 23, 59, 59, 999, new TimeSpan(-7, 0, 0)),
                        DateTimeOffset.Now
                    ),
                    new(
                        AuthorizationType.BigGameHuntingLicence_CanadianResident,
                        "41203",
                        "EHL-24202",
                        null,
                        null,
                        null,
                        new[] { "12", "14" },
                        null,
                        DateTimeOffset.Now,
                        DateTimeOffset.Now,
                        DateTimeOffset.Now
                    ),
                    new(
                        AuthorizationType.TrappingLicence_AssistantTrapper,
                        "50340",
                        "EAT-003103",
                        null,
                        null,
                        null,
                        Array.Empty<string>(),
                        "226",
                        DateTimeOffset.Now,
                        DateTimeOffset.Now,
                        DateTimeOffset.Now
                        )
                }
            };
        });
    }

    public override Task HandleAsync(GetAuthorizationsRequest req, CancellationToken ct)
    {
        var response = new GetAuthorizationsResponse
        {
            Authorizations = new List<Authorization>
            {
                new(
                    req.AuthorizationType
                    ?? AuthorizationType.BigGameHuntingLicence_CanadianResident,
                    "41203",
                    "EHL-24202",
                    null,
                    null,
                    null,
                    new[] { "12", "14" },
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

public class GetAuthorizationsRequest
{
    public AuthorizationType? AuthorizationType { get; set; }
    public DateTimeOffset? ModifiedSinceDateTime { get; set; }
}

public class GetAuthorizationsResponse
{
    public List<Authorization> Authorizations { get; set; }
}
