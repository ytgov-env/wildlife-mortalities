using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Services;
using Authorization = WildlifeMortalities.Data.Entities.Authorizations.Authorization;

namespace WildlifeMortalities.Test;

[Collection("Database Tests")]
public class MortalityTests
{
    //[Fact]
    //public async Task CanCreateBisonMortality()
    //{
    //    // Arrange
    //    var dbContextFactory = CreateTestDbContextFactory();
    //    var client = new Client
    //    {
    //        EnvClientId = "40405",
    //        Authorizations = new List<Authorization>()
    //        {
    //            new HuntingLicence
    //            {
    //                Seals = new List<Seal>()
    //                {
    //                    new Seal { Species = HuntedSpecies.WoodBison, Number = "S2105" }
    //                },
    //                StartDate = new DateTime(2021, 04, 01),
    //                EndDate = new DateTime(2022, 03, 31),
    //                Number = "HL5023"
    //            }
    //        }
    //    };
    //    using var context = dbContextFactory.CreateDbContext();
    //    var newClient = context.Add(client).Entity;
    //    await context.SaveChangesAsync();

    //    // Act
    //    var mortality = new WoodBisonMortality { Sex = Sex.Male };
    //    var service = new MortalityService(dbContextFactory);
    //    var createdMortality = await service.CreateMortality(mortality);
    //    context.Add(createdMortality.Value);
    //    await context.SaveChangesAsync();

    //    // Assert
    //    createdMortality.Value.Should().BeEquivalentTo(mortality);
    //}

    //[Fact]
    //public async Task CanGetBisonMortalityById()
    //{
    //    // Arrange
    //    var dbContextFactory = CreateTestDbContextFactory();
    //    using var context = dbContextFactory.CreateDbContext();

    //    var service = new MortalityService(dbContextFactory);

    //    var newMortality = await service.CreateMortality(
    //        new WoodBisonMortality() { Sex = Sex.Female }
    //    );
    //    context.Add(newMortality.Value);
    //    await context.SaveChangesAsync();

    //    // Act
    //    const int id = 1;
    //    var mortality = await service.GetMortalityById<WoodBisonMortality>(id);

    //    // Assert
    //    mortality.Id.Should().Be(id);
    //}

    [Fact]
    public async Task CannotGetMortalityByIdUsingMismatchedDerivedType()
    {
        // Arrange
        var dbContextFactory = CreateTestDbContextFactory();
        var service = new MortalityService(dbContextFactory);

        // Act
        const int id = 1;
        var mortality = await service.GetMortalityById<AmericanBlackBearMortality>(id);

        // Assert
        mortality.Should().Be(null);
    }

    //[Fact]
    //public async Task CanGetBisonMortalitiesByEnvClientId()
    //{
    //    // Arrange
    //    var dbContextFactory = CreateTestDbContextFactory();
    //    var service = new MortalityService(dbContextFactory);
    //    const string envClientId = "40405";
    //    var result = await service.CreateMortality(
    //        new WoodBisonMortality()
    //        {
    //            Reporter = new Client() { EnvClientId = envClientId },
    //            Sex = Sex.Unknown
    //        }
    //    );
    //    var mortality = result.Value;

    //    using var context = dbContextFactory.CreateDbContext();
    //    context.Add(mortality);
    //    await context.SaveChangesAsync();

    //    // Act
    //    var mortalities = await service.GetMortalitiesByEnvClientId<WoodBisonMortality>(
    //        envClientId
    //    );

    //    // Assert
    //    mortalities.Should().HaveCount(1);
    //}

    //[Fact]
    //public async Task CanGetBisonMortalitiesByConservationOfficerBadgeNumber()
    //{
    //    // Arrange
    //    var dbContextFactory = CreateTestDbContextFactory();
    //    var service = new MortalityService(dbContextFactory);

    //    const string badgeNumber = "43541";
    //    var conservationOfficer = new ConservationOfficer() { BadgeNumber = badgeNumber };
    //    //conservationOfficer.Mortalities.Add(new WoodBisonMortality());
    //    using var context = dbContextFactory.CreateDbContext();
    //    context.Add(conservationOfficer);
    //    await context.SaveChangesAsync();

    //    // Act
    //    var mortalities =
    //        await service.GetMortalitiesByConservationOfficerBadgeNumber<WoodBisonMortality>(
    //            badgeNumber
    //        );

    //    // Assert
    //    mortalities.Should().HaveCount(1);
    //}

    public static List<Client> CreateFourClients()
    {
        return new List<Client>()
        {
            new Client() { EnvClientId = "51230" },
            new Client() { EnvClientId = "40123" },
            new Client() { EnvClientId = "60102" },
            new Client() { EnvClientId = "20345" }
        };
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
