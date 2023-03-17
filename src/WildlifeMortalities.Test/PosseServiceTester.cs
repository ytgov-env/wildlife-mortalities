using RichardSzalay.MockHttp;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.Test;

public class PosseServiceTester
{
    private const string BaseAddress = "https://localhost";

    [Fact]
    public async Task GetClients_With22ApplicableAuthorizations_ShouldReturn22Authorizations()
    {
        // Arrange
        var storedClientResponse = await File.ReadAllTextAsync("TestData/22clients.json");

        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When(
                "https://localhost/clients?modifiedSinceDateTime=2023-03-16T14:30:10.0000000-07:00"
            )
            .Respond("application/json", storedClientResponse);
        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(BaseAddress);

        var service = new PosseService(client);
        var start = new DateTimeOffset(2023, 03, 16, 14, 30, 10, TimeSpan.FromHours(-7));

        // Act
        var response = await service.GetClients(start);

        // Assert
        response.Should().HaveCount(22);
    }
}
