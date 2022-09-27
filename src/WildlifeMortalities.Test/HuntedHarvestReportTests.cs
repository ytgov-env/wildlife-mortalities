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
public class IndividualHuntReportTests
{
    [Fact]
    public async void CanCreateIndividualHuntReport()
    {
        // Arrange
        var dbContextFactory = CreateTestDbContextFactory();
        using var context = dbContextFactory.CreateDbContext();
        var service = new IndividualHuntReportService<AmericanBlackBearMortality>(
            dbContextFactory,
            new MortalityService(dbContextFactory)
        );

        // Act
        var individualHuntReport = new IndividualHuntReport()
        {
            GmaSpecies = new GameManagementAreaSpecies()
            {
                Species = HuntedSpeciesWithGameManagementArea.AmericanBlackBear,
                GameManagementArea = new GameManagementArea() { Zone = "1", Subzone = "50" }
            },
            Mortality = new AmericanBlackBearMortality() { Sex = Sex.Male },
            Client = new Client() { EnvClientId = "50406" }
        };
        var result = await service.CreateIndividualHuntReport(individualHuntReport);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async void CannotCreateIndividualHuntReportWithoutAMortality()
    {
        // Arrange
        var dbContextFactory = CreateTestDbContextFactory();
        var service = new IndividualHuntReportService<AmericanBlackBearMortality>(
            dbContextFactory,
            new MortalityService(dbContextFactory)
        );

        // Act
        var individualHuntReport = new IndividualHuntReport();
        var result = await service.CreateIndividualHuntReport(individualHuntReport);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async void CanUpdateIndividualHuntReportWIthMortality()
    {
        // Arrange
        var dbContextFactory = CreateTestDbContextFactory();
        var service = new IndividualHuntReportService<AmericanBlackBearMortality>(
            dbContextFactory,
            new MortalityService(dbContextFactory)
        );

        var individualHuntReport = new IndividualHuntReport()
        {
            GmaSpecies = new GameManagementAreaSpecies()
            {
                Species = HuntedSpeciesWithGameManagementArea.AmericanBlackBear,
                GameManagementArea = new GameManagementArea() { Zone = "1", Subzone = "50" }
            },
            Mortality = new AmericanBlackBearMortality() { Sex = Sex.Male },
            Client = new Client() { EnvClientId = "50406" }
        };
        var createResult = await service.CreateIndividualHuntReport(individualHuntReport);
        individualHuntReport = await service.GetHarvestReportById(createResult.Value.Id);

        // Act
        individualHuntReport.Status = IndividualHuntReportStatus.Complete;
        var updateResult = await service.UpdateIndividualHuntReport(individualHuntReport);

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
