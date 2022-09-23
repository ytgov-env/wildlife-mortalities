using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Services;

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
            new MortalityService(dbContextFactory)
        );

        // Act
        var harvestReport = new HuntedHarvestReport()
        {
            TemporarySealNumber = "44064",
            GmaSpecies = new GameManagementAreaSpecies()
            {
                Species = HuntedSpeciesWithGameManagementArea.AmericanBlackBear,
                GameManagementArea = new GameManagementArea() { Zone = "1", Subzone = "50" }
            },
            Mortality = new AmericanBlackBearMortality() { Sex = Sex.Male },
            Client = new Client() { EnvClientId = "50406" }
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
            new MortalityService(dbContextFactory)
        );

        // Act
        var harvestReport = new HuntedHarvestReport() { TemporarySealNumber = "4404" };
        var result = await service.CreateHuntedHarvestReport(harvestReport);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async void CanUpdateHuntedHarvestReportWIthMortality()
    {
        // Arrange
        var dbContextFactory = CreateTestDbContextFactory();
        var service = new HuntedHarvestReportService<AmericanBlackBearMortality>(
            dbContextFactory,
            new MortalityService(dbContextFactory)
        );

        var harvestReport = new HuntedHarvestReport()
        {
            TemporarySealNumber = "44064",
            GmaSpecies = new GameManagementAreaSpecies()
            {
                Species = HuntedSpeciesWithGameManagementArea.AmericanBlackBear,
                GameManagementArea = new GameManagementArea() { Zone = "1", Subzone = "50" }
            },
            Mortality = new AmericanBlackBearMortality() { Sex = Sex.Male },
            Client = new Client() { EnvClientId = "50406" }
        };
        var createResult = await service.CreateHuntedHarvestReport(harvestReport);
        harvestReport = await service.GetHarvestReportById(createResult.Value.Id);

        // Act
        harvestReport.Status = HuntedHarvestReportStatus.Complete;
        var updateResult = await service.UpdateHuntedHarvestReport(harvestReport);

        // Assert
        updateResult.IsSuccess.Should().BeTrue();
    }

    public IDbContextFactory<AppDbContext> CreateTestDbContextFactory()
    {
        var config = AppSettings.GetConfiguration();
        var options = this.CreateUniqueClassOptions<AppDbContext>();
        var builder = new DbContextOptionsBuilder<AppDbContext>(options);
        builder
            .UseSqlServer(
                config.GetConnectionString("UnitTestConnection"),
                options => options.EnableRetryOnFailure()
            )
            .UseEnumCheckConstraints();
        return new TestDbContextFactory(builder.Options);
    }
}
