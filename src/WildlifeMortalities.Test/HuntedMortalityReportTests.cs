//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using WildlifeMortalities.Data;
//using WildlifeMortalities.Data.Entities;
//using WildlifeMortalities.Data.Entities.Mortalities;
//using WildlifeMortalities.Data.Entities.MortalityReports;
//using WildlifeMortalities.Data.Entities.People;
//using WildlifeMortalities.Data.Enums;
//using WildlifeMortalities.Shared.Services;

//namespace WildlifeMortalities.Test;

//[Collection("Database Tests")]
//public class HuntedMortalityReportTests
//{
//    [Fact]
//    public async void CanCreateHuntedMortalityReport()
//    {
//        // Arrange
//        var dbContextFactory = CreateTestDbContextFactory();
//        await using var context = dbContextFactory.CreateDbContext();
//        var service = new HuntedMortalityReportService<AmericanBlackBearMortality>(
//            dbContextFactory,
//            new MortalityService(dbContextFactory)
//        );

//        // Act
//        var huntedMortalityReport = new HuntedMortalityReport
//        {
//            GmaSpecies = new GameManagementAreaSpecies
//            {
//                Species = HuntedSpeciesWithGameManagementArea.AmericanBlackBear,
//                GameManagementArea = new GameManagementArea { Zone = "1", Subzone = "50" }
//            },
//            Mortality = new AmericanBlackBearMortality { Sex = Sex.Male },
//            Client = new Client { EnvClientId = "50406" }
//        };
//        var result = await service.CreateHuntedMortalityReport(huntedMortalityReport);

//        // Assert
//        result.IsSuccess.Should().BeTrue();
//    }

//    [Fact]
//    public async void CannotCreateHuntedMortalityReportWithoutAMortality()
//    {
//        // Arrange
//        var dbContextFactory = CreateTestDbContextFactory();
//        var service = new HuntedMortalityReportService<AmericanBlackBearMortality>(
//            dbContextFactory,
//            new MortalityService(dbContextFactory)
//        );

//        // Act
//        var huntedMortalityReport = new HuntedMortalityReport();
//        var result = await service.CreateHuntedMortalityReport(huntedMortalityReport);

//        // Assert
//        result.IsSuccess.Should().BeFalse();
//    }

//    [Fact]
//    public async void CanUpdateHuntedMortalityReportWIthMortality()
//    {
//        // Arrange
//        var dbContextFactory = CreateTestDbContextFactory();
//        var service = new HuntedMortalityReportService<AmericanBlackBearMortality>(
//            dbContextFactory,
//            new MortalityService(dbContextFactory)
//        );

//        var huntedMortalityReport = new HuntedMortalityReport
//        {
//            GmaSpecies = new GameManagementAreaSpecies
//            {
//                Species = HuntedSpeciesWithGameManagementArea.AmericanBlackBear,
//                GameManagementArea = new GameManagementArea { Zone = "1", Subzone = "50" }
//            },
//            Mortality = new AmericanBlackBearMortality { Sex = Sex.Male },
//            Client = new Client { EnvClientId = "50406" }
//        };
//        var createResult = await service.CreateHuntedMortalityReport(huntedMortalityReport);
//        huntedMortalityReport = await service.GetHarvestReportById(createResult.Value.Id);

//        // Act
//        huntedMortalityReport.Status = HuntedMortalityReportStatus.Complete;
//        var updateResult = await service.UpdateHuntedMortalityReport(huntedMortalityReport);

//        // Assert
//        updateResult.IsSuccess.Should().BeTrue();
//    }

//    public IDbContextFactory<AppDbContext> CreateTestDbContextFactory()
//    {
//        var config = AppSettings.GetConfiguration();
//        var options = this.CreateUniqueClassOptions<AppDbContext>();
//        var builder = new DbContextOptionsBuilder<AppDbContext>(options);
//        builder
//            .UseSqlServer(
//                config.GetConnectionString("UnitTestConnection"),
//                options => options.EnableRetryOnFailure()
//            )
//            .UseEnumCheckConstraints();
//        return new TestDbContextFactory(builder.Options);
//    }
//}
