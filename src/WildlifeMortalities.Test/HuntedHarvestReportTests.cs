using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Licences;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Services;
using Xunit;

namespace WildlifeMortalities.Test;

[Collection("Database Tests")]
public class HuntedHarvestReportTests
{
    [Fact]
    public async void CanCreateHuntedHarvestReport()
    {
        // Arrange
        var dbContextFactory = CreateTestDbContextFactory();
        using var context = dbContextFactory.CreateDbContext();
        var service = new HuntedHarvestReportService<AmericanBlackBearMortality>(
            dbContextFactory,
            new MortalityService<AmericanBlackBearMortality>(dbContextFactory)
        );

        // Act
        var harvestReport = new HuntedHarvestReport()
        {
            TemporarySealNumber = "4404",
            Mortality = new AmericanBlackBearMortality()
            {
                Reporter = new Client() { EnvClientId = "50406" },
                Sex = Sex.Male
            }
        };
        var result = await service.CreateHuntedHarvestReport(harvestReport);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async void CannotCreateHuntedHarvestReportWithoutAMortality()
    {
        // Arrange
        var dbContextFactory = CreateTestDbContextFactory();
        var service = new HuntedHarvestReportService<AmericanBlackBearMortality>(
            dbContextFactory,
            new MortalityService<AmericanBlackBearMortality>(dbContextFactory)
        );

        // Act
        var harvestReport = new HuntedHarvestReport() { TemporarySealNumber = "4404" };
        var result = await service.CreateHuntedHarvestReport(harvestReport);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    public IDbContextFactory<AppDbContext> CreateTestDbContextFactory()
    {
        var config = AppSettings.GetConfiguration();
        var options = this.CreateUniqueClassOptions<AppDbContext>();
        var builder = new DbContextOptionsBuilder<AppDbContext>(options);
        builder
            .UseSqlServer(
                config.GetConnectionString("UnitTestConnection"),
                options => options.UseNetTopologySuite().EnableRetryOnFailure()
            )
            .UseEnumCheckConstraints();
        return new TestDbContextFactory(builder.Options);
    }
}
