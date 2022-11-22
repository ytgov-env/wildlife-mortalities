using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.PosseExampleApi.Models;

namespace WildlifeMortalities.PosseExampleApi;

public class ClientsEndpoint : Endpoint<ClientsRequest, ClientsResponse>
{
    public override void Configure()
    {
        Get("/clients");
        Policies("ApiKey");
        Description(b => b
            .Produces<IEnumerable<ClientDto>>()
            .Produces(404));
    }

    public override async Task HandleAsync(ClientsRequest req, CancellationToken ct)
    {
        var response = new ClientsResponse
        {
            Clients = new List<ClientDto>
            {
                new(req.EnvClientId, "John", "Doe", new DateOnly(1984, 11, 25), DateTimeOffset.Now)
            }
        };

        await SendAsync(response);
    }
}

public class ClientsRequest
{
    public string? EnvClientId { get; set; }
    public DateTimeOffset? ModifiedSinceDateTime { get; set; }
}

public class ClientsResponse
{
    public List<ClientDto> Clients { get; set; }
}
