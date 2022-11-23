namespace WildlifeMortalities.PosseExampleApi.Features.Clients.GetByEnvClientId;

public class Endpoint : Endpoint<GetClientRequest, GetClientResponse>
{
    public override void Configure()
    {
        Get("/clients/{EnvClientId}");
        Policies("ApiKey");
        Description(b => b
            .Produces<Client>()
            .Produces(404));
        Summary(s =>
        {
            s.ResponseExamples[200] = new GetClientResponse
            {
                Client = new Client(
                    new[] { "432032" },
                    "John",
                    "Doe",
                    new DateOnly(1994, 11, 25),
                    DateTimeOffset.Now)
            };
        });
    }

    public override async Task HandleAsync(GetClientRequest req, CancellationToken ct)
    {
        var response = new GetClientResponse
        {
            Client = new Client(
                new[] { "43203" },
                "John",
                "Doe",
                new DateOnly(1984, 11, 25),
                DateTimeOffset.Now)
        };

        await SendAsync(response);
    }
}

public class GetClientRequest
{
    public string? EnvClientId { get; set; }
}

public class GetClientResponse
{
    public Client Client { get; set; }
}

public record Client(
    IEnumerable<string> PreviousEnvClientIds,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    DateTimeOffset LastModifiedDateTime
);
