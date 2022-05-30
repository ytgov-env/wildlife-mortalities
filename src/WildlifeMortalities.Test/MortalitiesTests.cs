using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Data.Entities.Licences;
using WildlifeMortalities.Data.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;
using AutoBogus;

namespace WildlifeMortalities.Test;

public class MortalitiesTests
{
    [Fact]
    public async Task CanCreateBisonMortality()
    {
        var dbContextFactory = new TestDbContextFactory();
        var context = dbContextFactory.CreateDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var client = new Client
        {
            EnvClientId = "40405",
            Licences = new List<Licence>()
            {
                new HuntingLicence {
                    Seals = new List<Seal>()
                            {
                                new Seal { Species = HuntedSpecies.Bison, Number = "S2105" }
                            },
                    StartDate = new DateTime(2021, 04, 01),
                    EndDate = new DateTime(2022, 03, 31),
                    Number = "HL5023"
                }
            }
        };
        var newClient = context.Add(client).Entity;
        await context.SaveChangesAsync();

        var service = new MortalityService<WoodBisonMortality>(dbContextFactory);

        var mortality = new WoodBisonMortality { Sex = Sex.Male, ReporterId = newClient.Id };
        var createdMortality = await service.CreateMortality(mortality);
        createdMortality.Should().BeEquivalentTo(mortality);
    }

    [Fact]
    public async Task CanGetBisonMortalityById()
    {
        var dbContextFactory = new TestDbContextFactory();
        var service = new MortalityService<WoodBisonMortality>(dbContextFactory);

        const int id = 1;
        var mortality = await service.GetMortalityById(id);
        mortality.Id.Should().Be(id);
    }

    [Fact]
    public async Task CannotGetMortalityByIdUsingMismatchedDerivedType()
    {
        var dbContextFactory = new TestDbContextFactory();
        var service = new MortalityService<AmericanBlackBearMortality>(dbContextFactory);

        const int id = 1;
        var mortality = await service.GetMortalityById(id);
        mortality.Should().Be(null);
    }

    [Fact]
    public async Task CanGetBisonMortalitiesByEnvClientId()
    {
        var dbContextFactory = new TestDbContextFactory();
        var service = new MortalityService<WoodBisonMortality>(dbContextFactory);

        const string envClientId = "40405";
        var mortalities = await service.GetMortalitiesByEnvClientId(envClientId);
        mortalities.Should().HaveCount(1);
    }

    [Fact]
    public async Task CanGetBisonMortalitiesByConservationOfficerBadgeNumber()
    {
        var dbContextFactory = new TestDbContextFactory();
        var service = new MortalityService<WoodBisonMortality>(dbContextFactory);

        const string badgeNumber = "40405";
        var mortalities = await service.GetMortalitiesByConservationOfficerBadgeNumber(badgeNumber);
        mortalities.Should().HaveCount(1);
    }


}
