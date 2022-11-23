namespace WildlifeMortalities.PosseExampleApi.Features.Clients.GetAll;

public class Endpoint : Endpoint<GetClientsRequest, GetClientsResponse>
{
    public override void Configure()
    {
        Get("/clients");
        Policies("ApiKey");
        Description(b => b
            .Produces<IEnumerable<Client>>());
        Summary(s =>
        {
            s.ResponseExamples[200] = new GetClientsResponse
            {
                Clients = new List<Client>
                {
                    new(
                        "256402",
                        new[] { "432035", "503100" },
                        "John",
                        "Doe",
                        new DateOnly(2001, 1, 2),
                        DateTimeOffset.Now),
                    new(
                        "253020",
                        Array.Empty<string>(),
                        "Jane",
                        "Doe",
                        new DateOnly(1984, 11, 25),
                        DateTimeOffset.Now)
                }
            };
        });
    }

    public override async Task HandleAsync(GetClientsRequest req, CancellationToken ct)
    {
        var response = new GetClientsResponse
        {
            Clients = new List<Client>
            {
                new(
                    "56402",
                    new[] { "43203" },
                    "John",
                    "Doe",
                    new DateOnly(1984, 11, 25),
                    DateTimeOffset.Now),
                new(
                    "23020",
                    Array.Empty<string>(),
                    "John",
                    "Doe",
                    new DateOnly(1984, 11, 25),
                    DateTimeOffset.Now)
            }
        };

        await SendAsync(response);
    }
}

public class GetClientsRequest
{
    public DateTimeOffset? ModifiedSinceDateTime { get; set; }
}

public class GetClientsResponse
{
    public List<Client> Clients { get; set; }
}

public record Client(
    string EnvClientId,
    IEnumerable<string> PreviousEnvClientIds,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    DateTimeOffset LastModifiedDateTime
);
