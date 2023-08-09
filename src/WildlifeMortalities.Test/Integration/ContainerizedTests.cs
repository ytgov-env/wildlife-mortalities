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
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports;
using static WildlifeMortalities.Data.Entities.Violation;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.App.Features.Shared.Mortalities.Caribou;
using Bogus;
using System.Text.Json;

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
    public async Task UpdateReport_WithSameValues_NoChangeInViolations()
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
                    Sex = Sex.Male
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
    public async Task SpecialGuidedReport_DeleteMortality_BioSubmissionIsDeleted()
    {
        var hunter = new Client
        {
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0),
            FirstName = "Test",
            LastName = "User",
            EnvPersonId = "410302"
        };

        var guide = new Client
        {
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0),
            FirstName = "Test",
            LastName = "User",
            EnvPersonId = "323123"
        };

        await _context.People.AddRangeAsync(hunter, guide);
        await _context.SaveChangesAsync();

        var gma = await _context.GameManagementAreas.SingleAsync(gma => gma.Area == "5-01");
        var report = new SpecialGuidedHuntReport
        {
            HuntStartDate = new DateTime(2023, 10, 11, 0, 0, 0),
            HuntEndDate = new DateTime(2023, 10, 13, 0, 0, 0),
            HuntedActivities = new()
            {
                new HuntedActivity()
                {
                    Mortality = new CaribouMortality()
                    {
                        DateOfDeath = new DateTimeOffset(
                            2023,
                            10,
                            12,
                            0,
                            0,
                            0,
                            TimeSpan.FromHours(-7)
                        ),
                        LegalHerd = CaribouMortality.CaribouHerd.Fortymile,
                        Sex = Sex.Male
                    },
                    GameManagementAreaId = gma.Id,
                },
                new HuntedActivity()
                {
                    Mortality = new CoyoteMortality()
                    {
                        DateOfDeath = new DateTimeOffset(
                            2023,
                            10,
                            12,
                            0,
                            0,
                            0,
                            TimeSpan.FromHours(-7)
                        ),
                        Sex = Sex.Male
                    },
                    GameManagementAreaId = gma.Id,
                }
            },
            ClientId = hunter.Id,
            Result = GuidedHuntResult.WentHuntingAndKilledWildlife,
            GuideId = guide.Id
        };

        var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
        contextFactory
            .Setup(contextFactory => contextFactory.CreateDbContext())
            .Returns(GetContext);
        var service = new MortalityService(contextFactory.Object);

        await service.CreateReport(report, _seedUser.Id);

        var reportFromDb = await _context.Reports
            .WithEntireGraph()
            .FirstAsync(x => x.Id == report.Id);

        _context.Activities.Should().HaveCount(2);
        _context.BioSubmissions.Should().HaveCount(1);

        var secondReportViewModel = new SpecialGuidedHuntReportViewModel(
            (SpecialGuidedHuntReport)reportFromDb
        );
        secondReportViewModel.HuntedActivityViewModels.RemoveAt(0);

        var secondReport = secondReportViewModel.GetReport(hunter.Id);

        await service.UpdateReport(secondReport, _seedUser.Id);

        _context.Activities.Should().HaveCount(1);
        _context.BioSubmissions.Should().BeEmpty();
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

        var bioSubmission = await _context.BioSubmissions.SingleOrDefaultAsync();
        bioSubmission.Should().BeAssignableTo<CaribouBioSubmission>();
    }

    [Fact]
    public async Task Caribou_ChangeFromHerdWithBioSubToHerdWithoutBioSub_NoExceptions()
    {
        var hunter = new Client
        {
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0),
            FirstName = "Test",
            LastName = "User",
        };

        await _context.People.AddAsync(hunter);
        await _context.SaveChangesAsync();

        var fortyMileGma = await _context.GameManagementAreas.SingleAsync(x => x.Area == "5-01");
        var porcupineGma = await _context.GameManagementAreas.SingleAsync(x => x.Area == "1-60");
        var report = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(2023, 10, 12, 0, 0, 0, TimeSpan.FromHours(-7)),
                    LegalHerd = CaribouMortality.CaribouHerd.Fortymile,
                    Sex = Sex.Male
                },
                GameManagementAreaId = fortyMileGma.Id,
            },
            PersonId = hunter.Id,
        };

        var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
        contextFactory
            .Setup(contextFactory => contextFactory.CreateDbContext())
            .Returns(GetContext);
        var service = new MortalityService(contextFactory.Object);

        await service.CreateReport(report, _seedUser.Id);

        var bioSubmission = await _context.BioSubmissions.AsNoTracking().SingleOrDefaultAsync();

        var mortality = report.Activity.Mortality as CaribouMortality;
        mortality!.LegalHerd = CaribouMortality.CaribouHerd.Porcupine;
        report.Activity.GameManagementAreaId = porcupineGma.Id;

        await service.UpdateReport(report, _seedUser.Id);

        var bioSubmissions = await _context.BioSubmissions.AsNoTracking().ToListAsync();
        bioSubmissions.Should().BeEmpty();
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

        var area = await _context.GameManagementAreas.SingleAsync(gma => gma.Area == "1-60");
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
                GameManagementAreaId = area.Id,
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
            x => ((HuntingBagLimitEntry)x.BagLimitEntry).Areas.Any(y => y.Id == area.Id)
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

        foreach (
            var bagEntry in await _context.BagEntries
                .AsNoTracking()
                .Where(x => x.BagLimitEntry.Species == Species.Caribou)
                .ToListAsync()
        )
        {
            bagEntry.TotalValue.Should().Be(0);
        }
    }

    [Fact]
    public async Task UpdateBioSubmission_ProvideAllRequiredSamples_ShouldRemoveBioSubmissionMissingViolation()
    {
        var hunter = new Client
        {
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0),
            FirstName = "Test",
            LastName = "User",
        };

        await _context.People.AddAsync(hunter);
        await _context.SaveChangesAsync();

        var fortyMileGma = await _context.GameManagementAreas.SingleAsync(x => x.Area == "5-01");
        var report = new IndividualHuntedMortalityReport
        {
            Activity = new HuntedActivity()
            {
                Mortality = new CaribouMortality()
                {
                    DateOfDeath = new DateTimeOffset(2023, 5, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                    LegalHerd = CaribouMortality.CaribouHerd.Fortymile,
                    Sex = Sex.Male
                },
                GameManagementAreaId = fortyMileGma.Id,
            },
            PersonId = hunter.Id,
        };

        var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
        contextFactory
            .Setup(contextFactory => contextFactory.CreateDbContext())
            .Returns(GetContext);
        var service = new MortalityService(contextFactory.Object);

        await service.CreateReport(report, _seedUser.Id);

        var bioSubmission = await _context.BioSubmissions
            .OfType<CaribouBioSubmission>()
            .AsNoTracking()
            .SingleAsync();
        bioSubmission.IsIncisorBarProvided = false;
        bioSubmission.UpdateRequiredOrganicMaterialsStatus();
        await service.UpdateOrganicMaterialForBioSubmission(bioSubmission, report.Id, _seedUser.Id);

        _context.Violations
            .Should()
            .ContainSingle(x => x.Rule == RuleType.AllRequiredSamplesNotSubmitted);

        bioSubmission.IsIncisorBarProvided = true;
        bioSubmission.UpdateRequiredOrganicMaterialsStatus();
        await service.UpdateOrganicMaterialForBioSubmission(bioSubmission, report.Id, _seedUser.Id);

        _context.Violations
            .Should()
            .NotContain(x => x.Rule == RuleType.AllRequiredSamplesNotSubmitted);
    }

    [Fact]
    public async Task ThresholdRuleProcess_WithThreshold_And_ThresholdExceeded_GeneratesViolations()
    {
        var hunter = new Client
        {
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0),
            FirstName = "Test",
            LastName = "User",
        };

        await _context.People.AddAsync(hunter);
        await _context.SaveChangesAsync();

        var gma = await _context.GameManagementAreas.SingleAsync(x => x.Area == "2-59");

        var activitiesBeforeThresholdMet = Enumerable
            .Range(0, 5)
            .Select(
                _ =>
                    new IndividualHuntedMortalityReport
                    {
                        Activity = new HuntedActivity()
                        {
                            Mortality = new MooseMortality()
                            {
                                DateOfDeath = new DateTimeOffset(
                                    2023,
                                    10,
                                    12,
                                    0,
                                    0,
                                    0,
                                    TimeSpan.FromHours(-7)
                                ),
                                Sex = Sex.Male
                            },
                            GameManagementAreaId = gma.Id
                        },
                        PersonId = hunter.Id,
                    }
            )
            .ToList();

        var activitiesOnDayThresholdMet = Enumerable
            .Range(0, 6)
            .Select(
                _ =>
                    new IndividualHuntedMortalityReport
                    {
                        Activity = new HuntedActivity()
                        {
                            Mortality = new MooseMortality()
                            {
                                DateOfDeath = new DateTimeOffset(
                                    2023,
                                    10,
                                    13,
                                    0,
                                    0,
                                    0,
                                    TimeSpan.FromHours(-7)
                                ),
                                Sex = Sex.Male
                            },
                            GameManagementAreaId = gma.Id
                        },
                        PersonId = hunter.Id,
                    }
            )
            .ToList();

        var activitiesTwoDaysAfterThresholdMet = Enumerable
            .Range(0, 3)
            .Select(
                _ =>
                    new IndividualHuntedMortalityReport
                    {
                        Activity = new HuntedActivity()
                        {
                            Mortality = new MooseMortality()
                            {
                                DateOfDeath = new DateTimeOffset(
                                    2023,
                                    10,
                                    15,
                                    0,
                                    0,
                                    0,
                                    TimeSpan.FromHours(-7)
                                ),
                                Sex = Sex.Male
                            },
                            GameManagementAreaId = gma.Id
                        },
                        PersonId = hunter.Id,
                    }
            )
            .ToList();

        var activitiesThreeDaysAfterThresholdMet = Enumerable
            .Range(0, 2)
            .Select(
                _ =>
                    new IndividualHuntedMortalityReport
                    {
                        Activity = new HuntedActivity()
                        {
                            Mortality = new MooseMortality()
                            {
                                DateOfDeath = new DateTimeOffset(
                                    2023,
                                    10,
                                    16,
                                    0,
                                    0,
                                    0,
                                    TimeSpan.FromHours(-7)
                                ),
                                Sex = Sex.Male
                            },
                            GameManagementAreaId = gma.Id
                        },
                        PersonId = hunter.Id,
                    }
            )
            .ToList();

        foreach (
            var item in activitiesBeforeThresholdMet
                .Union(activitiesThreeDaysAfterThresholdMet)
                .Union(activitiesOnDayThresholdMet)
                .Union(activitiesTwoDaysAfterThresholdMet)
        )
        {
            var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
            contextFactory
                .Setup(contextFactory => contextFactory.CreateDbContext())
                .Returns(GetContext);
            var service = new MortalityService(contextFactory.Object);

            await service.CreateReport(item, _seedUser.Id);
        }

        foreach (var item in activitiesBeforeThresholdMet)
        {
            _context.Violations
                .Where(
                    x => x.Activity.Id == item.Activity.Id && x.Rule == RuleType.ThresholdExceeded
                )
                .Should()
                .BeEmpty();
        }

        foreach (
            var item in activitiesOnDayThresholdMet
                .Union(activitiesTwoDaysAfterThresholdMet)
                .ToList()
        )
        {
            var violations = await _context.Violations
                .Where(
                    x => x.Activity.Id == item.Activity.Id && x.Rule == RuleType.ThresholdExceeded
                )
                .ToListAsync();

            violations.Should().ContainSingle();

            var violation = violations[0];

            violation.Rule.Should().Be(RuleType.ThresholdExceeded);
            violation.Severity.Should().Be(SeverityType.PotentiallyIllegal);
            violation.ActivityId.Should().Be(item.Activity.Id);
            violation.Description
                .Should()
                .Be(
                    "Threshold exceeded for moose in 2-56, 2-58, 2-59, 2-62, 2-63, 4-04, 4-05, 4-06. Threshold of 11 was reached on 2023-10-13."
                );
        }

        foreach (var item in activitiesThreeDaysAfterThresholdMet)
        {
            var violations = await _context.Violations
                .Where(
                    x => x.Activity.Id == item.Activity.Id && x.Rule == RuleType.ThresholdExceeded
                )
                .ToListAsync();

            violations.Should().ContainSingle();

            var violation = violations[0];

            violation.Rule.Should().Be(RuleType.ThresholdExceeded);
            violation.Severity.Should().Be(SeverityType.Illegal);
            violation.ActivityId.Should().Be(item.Activity.Id);
            violation.Description
                .Should()
                .Be(
                    "Threshold exceeded for moose in 2-56, 2-58, 2-59, 2-62, 2-63, 4-04, 4-05, 4-06. Threshold of 11 was reached on 2023-10-13."
                );
        }
    }

    [Fact]
    public async Task DraftOutfitterGuidedReport_CanConverToRealReport()
    {
        var hunter = new Client
        {
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0),
            FirstName = "Test",
            LastName = "User",
            EnvPersonId = "410302"
        };

        _context.People.Add(hunter);
        await _context.SaveChangesAsync();

        var outfitterArea = await _context.OutfitterAreas.FirstAsync();
        var gma = await _context.GameManagementAreas.FirstAsync();
        var reportViewModel = new OutfitterGuidedHuntReportViewModel()
        {
            ChiefGuide = new() { FirstName = "Test", LastName = "User" },
            OutfitterArea = outfitterArea,
            Result = GuidedHuntResult.WentHuntingAndKilledWildlife,
            HuntedActivityViewModels = new()
            {
                new()
                {
                    GameManagementArea = gma,
                    HrbsNumber = "50503",
                    Seal = "4020",
                    MortalityWithSpeciesSelectionViewModel = new()
                    {
                        MortalityViewModel = new CaribouMortalityViewModel()
                        {
                            BioSubmission = null,
                            DateOfDeath = null,
                            Sex = Sex.Male,
                            IsDraft = true,
                            LegalHerd = CaribouMortality.CaribouHerd.Porcupine
                        },
                        Species = Species.Caribou
                    }
                }
            }
        };

        var contextFactory = new Mock<IDbContextFactory<AppDbContext>>();
        contextFactory
            .Setup(contextFactory => contextFactory.CreateDbContext())
            .Returns(GetContext);
        var service = new MortalityService(contextFactory.Object);
        var content = JsonSerializer.Serialize(
            new MortalityReportPageViewModel
            {
                ReportType = ReportType.OutfitterGuidedHuntReport,
                ReportViewModel = reportViewModel,
            }
        );

        var draftReportId = await service.CreateDraftReport(
            nameof(OutfitterGuidedHuntReport),
            content,
            1
        );

        var draft = await _context.DraftReports.FirstOrDefaultAsync(x => x.Id == draftReportId);
        if (draft == null)
        {
            return;
        }

        var reportPageViewModel = JsonSerializer.Deserialize<MortalityReportPageViewModel>(
            draft.SerializedData
        )!;

        reportViewModel = (OutfitterGuidedHuntReportViewModel)reportPageViewModel.ReportViewModel;

        reportViewModel.HuntingDateRange = new MudBlazor.DateRange(
            new DateTime(2023, 8, 1),
            new DateTime(2023, 8, 6)
        );
        foreach (var item in reportViewModel.HuntedActivityViewModels)
        {
            item.MortalityWithSpeciesSelectionViewModel.MortalityViewModel.DateOfDeath =
                new DateTime(2023, 8, 3);
        }

        var report = reportPageViewModel.ReportViewModel.GetReport(hunter.Id);
        var reportId = await service.CreateReport(report, _seedUser.Id, draftReportId);

        _context.DraftReports.Should().BeEmpty();
        _context.Reports.Should().ContainSingle();
    }
}
