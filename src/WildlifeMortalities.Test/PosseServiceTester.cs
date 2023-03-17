using Microsoft.EntityFrameworkCore;
using RichardSzalay.MockHttp;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.Test;

public class PosseServiceTester
{
    private const string BaseAddress = "https://localhost";
    private const string AuthorizationsUri =
        "https://localhost/authorizations?modifiedSinceDateTime=2020-01-01T00:00:00.0000000-07:00";
    private static readonly DateTimeOffset s_authorizationsTimestamp = new DateTimeOffset(
        2020,
        1,
        1,
        0,
        0,
        0,
        TimeSpan.FromHours(-7)
    );

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

    [Fact]
    public async Task GetAuthorizations_WithInvalidEnvClientId_ShouldDiscardAuthorization()
    {
        // Arrange
        var storedClientResponse = await File.ReadAllTextAsync(
            "TestData/GetAuthorizations_WithInvalidEnvClientId_ShouldDiscardAuthorization.json"
        );

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(AuthorizationsUri).Respond("application/json", storedClientResponse);
        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(BaseAddress);

        var service = new PosseService(client);

        Dictionary<string, Client> clientMapper = new Dictionary<string, Client>
        {
            {
                "523203",
                new Client { EnvClientId = "523203" }
            }
        };

        DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase(
            "GetAuthorizations_WithInvalidEnvClientId_ShouldDiscardAuthorization"
        );

        AppDbContext context = new AppDbContext(builder.Options);

        // Act
        var response = await service.GetAuthorizations(
            s_authorizationsTimestamp,
            clientMapper,
            context
        );

        // Assert
        response.Should().ContainSingle();

        var authorization = response.First().Item1;
        authorization
            .Should()
            .BeAssignableTo<SmallGameHuntingLicence>()
            .Subject.Number.Should()
            .Be("EHL-9784");
    }
}
