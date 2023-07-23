using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.DataSeeder;
using WildlifeMortalities.Shared.Services;
using Moq;
using WildlifeMortalities.Data.Entities.Users;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services.Reports.Single;
using WildlifeMortalities.App.Features.Reports;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;

namespace WildlifeMortalities.Test.Integration;

public class ContainerizedTests : IAsyncLifetime
{
    private IContainer _container = null!;
    private User _seedUser = null!;
    private AppDbContext _context = null!;

    private AppDbContext GetContext()
    {
        var connectionString =
            $"server={_container.Hostname};user id={MsSqlBuilder.DefaultUsername};password={MsSqlBuilder.DefaultPassword};database={MsSqlBuilder.DefaultDatabase};TrustServerCertificate=True";
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .EnableSensitiveDataLogging()
            .Options;

        return new AppDbContext(options);
    }

    public async Task InitializeAsync()
    {
        _container = new MsSqlBuilder().WithPortBinding(1433, 1433).Build();
        await _container.StartAsync();

        _context = GetContext();
        await _context.Database.MigrateAsync();
        await Seeder.Seed(_context);

        _seedUser = new User
        {
            FirstName = "Test",
            LastName = "User",
            NameIdentifier = "testuser",
            EmailAddress = "etst2@egwtge.com",
            FullName = "Test User",
            Settings = new UserSettings() { IsDarkMode = false }
        };

        _context.Users.Add(_seedUser);
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }

    [Fact]
    public async Task MyTest()
    {
        var hunter = new Client
        {
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0),
            FirstName = "Test",
            LastName = "User",
        };

        await _context.People.AddAsync(hunter);
        await _context.SaveChangesAsync();

        var gma = await _context.GameManagementAreas.SingleAsync(gma => gma.Area == "1-60");
        var report = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(2023, 10, 12, 0, 0, 0, TimeSpan.FromHours(-7)),
                    LegalHerd = CaribouMortality.CaribouHerd.Porcupine,
                    Sex = Data.Enums.Sex.Male
                },
                GameManagementAreaId = gma.Id,
            },
            PersonId = hunter.Id,
        };

        var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
        contextFactory
            .Setup(contextFactory => contextFactory.CreateDbContext())
            .Returns(GetContext);
        var service = new MortalityService(contextFactory.Object);

        await service.CreateReport(report, _seedUser.Id);

        var violationsBeforeUpdate = await _context.Violations.ToListAsync();

        var reportFromDb = await _context.Reports
            .WithEntireGraph()
            .FirstAsync(x => x.Id == report.Id);

        var secondReportViewModel = new IndividualHuntedMortalityReportViewModel(
            (IndividualHuntedMortalityReport)reportFromDb
        );
        var secondReport = secondReportViewModel.GetReport(hunter.Id);

        await service.UpdateReport(secondReport, _seedUser.Id);

        var violationsAfterUpdate = await _context.Violations.ToListAsync();

        violationsBeforeUpdate
            .Should()
            .BeEquivalentTo(violationsAfterUpdate, config => config.Excluding(y => y.Id));
    }

    [Fact]
    public async Task Caribou_ChangeFromHerdWithoutBioSubToHerdWithBioSub_NoExceptions()
    {
        var hunter = new Client
        {
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0),
            FirstName = "Test",
            LastName = "User",
        };

        await _context.People.AddAsync(hunter);
        await _context.SaveChangesAsync();

        var porcupineGma = await _context.GameManagementAreas.SingleAsync(x => x.Area == "1-60");
        var report = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(2023, 10, 12, 0, 0, 0, TimeSpan.FromHours(-7)),
                    LegalHerd = CaribouMortality.CaribouHerd.Porcupine,
                    Sex = Sex.Male
                },
                GameManagementAreaId = porcupineGma.Id,
            },
            PersonId = hunter.Id,
        };

        var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
        contextFactory
            .Setup(contextFactory => contextFactory.CreateDbContext())
            .Returns(GetContext);
        var service = new MortalityService(contextFactory.Object);

        await service.CreateReport(report, _seedUser.Id);

        var fortyMileGma = await _context.GameManagementAreas.SingleAsync(x => x.Area == "5-01");
        report.Activity.GameManagementAreaId = fortyMileGma.Id;

        var mortality = report.Activity.Mortality as CaribouMortality;
        mortality!.LegalHerd = CaribouMortality.CaribouHerd.Fortymile;

        await service.UpdateReport(report, _seedUser.Id);
    }

    [Fact]
    public async Task ChangeMortalityType_BagLimitIsDecreased()
    {
        var hunter = new Client
        {
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0),
            FirstName = "Test",
            LastName = "User",
        };

        await _context.People.AddAsync(hunter);
        await _context.SaveChangesAsync();

        var gma = await _context.GameManagementAreas.SingleAsync(gma => gma.Area == "1-60");
        var report = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(2023, 10, 12, 0, 0, 0, TimeSpan.FromHours(-7)),
                    LegalHerd = CaribouMortality.CaribouHerd.Porcupine,
                    Sex = Data.Enums.Sex.Male
                },
                GameManagementAreaId = gma.Id,
            },
            PersonId = hunter.Id,
        };

        var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
        contextFactory
            .Setup(contextFactory => contextFactory.CreateDbContext())
            .Returns(GetContext);
        var service = new MortalityService(contextFactory.Object);

        await service.CreateReport(report, _seedUser.Id);

        var bagLimitEntry = await _context.BagEntries.FirstAsync(
            x => ((HuntingBagLimitEntry)x.BagLimitEntry).Areas.Any(y => y.Id == gma.Id)
        );

        bagLimitEntry.CurrentValue.Should().Be(1);

        var reportFromDb = await _context.Reports
            .WithEntireGraph()
            .FirstAsync(x => x.Id == report.Id);

        var secondReportViewModel = new IndividualHuntedMortalityReportViewModel(
            (IndividualHuntedMortalityReport)reportFromDb
        );

        secondReportViewModel.SpeciesChanged(
            secondReportViewModel
                .HuntedActivityViewModel
                .MortalityWithSpeciesSelectionViewModel
                .MortalityViewModel
                .Id!
                .Value,
            Species.AmericanBlackBear
        );

        var secondReport = secondReportViewModel.GetReport(hunter.Id);

        await service.UpdateReport(secondReport, _seedUser.Id);

        var violationsAfterUpdate = await _context.Violations.ToListAsync();

        foreach (var item in await _context.BagEntries.AsNoTracking().ToListAsync())
        {
            item.TotalValue.Should().Be(0);
        }
    }
}
