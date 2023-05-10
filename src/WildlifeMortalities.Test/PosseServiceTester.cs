using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RichardSzalay.MockHttp;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Shared.Services.Posse;

namespace WildlifeMortalities.Test;

public class PosseServiceTester
{
    private const string BaseAddress = "https://localhost";
    private const string AuthorizationsUri =
        "https://localhost/authorizations?modifiedSinceDateTime=2020-01-01T00:00:00.0000000-07:00";
    private static readonly DateTimeOffset s_authorizationsTimestamp =
        new(2020, 1, 1, 0, 0, 0, TimeSpan.FromHours(-7));

    [Fact]
    public async Task GetClients_With22ApplicableAuthorizations_ShouldReturn22Authorizations()
    {
        const string TestName = nameof(
            GetClients_With22ApplicableAuthorizations_ShouldReturn22Authorizations
        );
        // Arrange
        var storedClientResponse = await File.ReadAllTextAsync($"TestData/{TestName}.json");

        var mockHttp = new MockHttpMessageHandler();
        mockHttp
            .When(
                "https://localhost/clients?modifiedSinceDateTime=2023-03-16T14:30:10.0000000-07:00"
            )
            .Respond("application/json", storedClientResponse);
        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(BaseAddress);

        var config = new ConfigurationBuilder().Build();
        var service = new PosseService(client, config);
        var start = new DateTimeOffset(2023, 03, 16, 14, 30, 10, TimeSpan.FromHours(-7));

        // Act
        var response = await service.GetClients(start);

        // Assert
        response.Should().HaveCount(22);
    }

    [Fact]
    public async Task GetAuthorizations_WithInvalidEnvClientId_ShouldDiscardAuthorization()
    {
        const string TestName = nameof(
            GetAuthorizations_WithInvalidEnvClientId_ShouldDiscardAuthorization
        );
        // Arrange
        var storedClientResponse = await File.ReadAllTextAsync($"TestData/{TestName}.json");

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(AuthorizationsUri).Respond("application/json", storedClientResponse);
        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri(BaseAddress);

        var config = new ConfigurationBuilder().Build();
        var service = new PosseService(client, config);

        var clientMapper = new Dictionary<string, PersonWithAuthorizations>
        {
            {
                "523203",
                new Client { EnvPersonId = "523203" }
            }
        };

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase(TestName);

        var context = new AppDbContext(builder.Options);

        context.Seasons.Add(new HuntingSeason(2021));
        await context.SaveChangesAsync();

        // Act
        var response = await service.GetAuthorizations(
            s_authorizationsTimestamp,
            clientMapper,
            context
        );

        // Assert
        response.Should().ContainSingle();

        var authorization = response.First();
        authorization
            .Should()
            .BeAssignableTo<SmallGameHuntingLicence>()
            .Subject.Number.Should()
            .Be("EHL-9784");
    }
}
