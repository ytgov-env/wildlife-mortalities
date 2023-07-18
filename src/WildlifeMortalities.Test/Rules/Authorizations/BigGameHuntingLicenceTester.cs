using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.People;
using static WildlifeMortalities.Data.Entities.Violation;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Shared.Services.Rules.Authorizations;

namespace WildlifeMortalities.Test.Rules.Authorizations;

public class BigGameHuntingLicenceTester
{
    [Fact]
    public async Task AuthorizationRule_IsNotBigGame_IsNotApplicable()
    {
        var dbContext = new AppDbContext();

        var report = new IndividualHuntedMortalityReport()
        {
            HuntedActivity = new HuntedActivity()
            {
                Mortality = new ArcticGroundSquirrelMortality()
            }
        };

        var item = new BigGameHuntingLicenceRulePipelineItem();

        var context = new AuthorizationRulePipelineContext(dbContext);

        var result = await item.Process(report, context);

        result.Should().BeTrue();
        context.Violations.Should().BeEmpty();
        context.RelevantAuthorizations.Should().BeEmpty();
    }

    [Fact]
    public async Task AuthorizationRule_Trapping_IsNotApplicable()
    {
        var dbContext = new AppDbContext();

        var report = new TrappedMortalitiesReport();

        var item = new BigGameHuntingLicenceRulePipelineItem();

        var context = new AuthorizationRulePipelineContext(dbContext);

        var result = await item.Process(report, context);

        result.Should().BeTrue();
        context.Violations.Should().BeEmpty();
        context.RelevantAuthorizations.Should().BeEmpty();
    }

    [Fact]
    public async Task AuthorizationRule_WithNoAuthorization_IsIllegal()
    {
        var dbContext = new AppDbContext();

        var report = new IndividualHuntedMortalityReport()
        {
            Person = new Client() { Authorizations = new() },
            HuntedActivity = new HuntedActivity()
            {
                Mortality = new AmericanBlackBearMortality()
                {
                    DateOfDeath = new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero)
                }
            }
        };

        var item = new BigGameHuntingLicenceRulePipelineItem();

        var context = new AuthorizationRulePipelineContext(dbContext);

        var result = await item.Process(report, context);

        result.Should().BeTrue();
        context.Violations.Should().ContainSingle();
        var violation = context.Violations.First();

        violation
            .Should()
            .BeEquivalentTo(
                new Violation(
                    report.GetActivities().First(),
                    RuleType.NoValidBigGameHuntingLicence,
                    SeverityType.Illegal,
                    "Does not have a valid big game hunting licence on 2023-04-01."
                )
            );

        context.RelevantAuthorizations.Should().BeEmpty();
    }

    [Fact]
    public async Task AuthorizationRule_WithAuthorization_IsLegal()
    {
        var dbContext = new AppDbContext();
        var dateOfDeath = new DateTimeOffset(2023, 6, 1, 0, 0, 0, TimeSpan.FromHours(-7));
        var validStart = dateOfDeath.AddDays(-10);
        var validEnd = dateOfDeath.AddDays(10);

        var report = new IndividualHuntedMortalityReport()
        {
            Person = new Client()
            {
                Authorizations = new()
                {
                    new BigGameHuntingLicence()
                    {
                        Season = new HuntingSeason(2023),
                        ValidFromDateTime = validStart,
                        ValidToDateTime = validEnd
                    }
                }
            },
            HuntedActivity = new HuntedActivity()
            {
                Mortality = new AmericanBlackBearMortality() { DateOfDeath = dateOfDeath }
            }
        };

        var item = new BigGameHuntingLicenceRulePipelineItem();

        var context = new AuthorizationRulePipelineContext(dbContext);

        var result = await item.Process(report, context);

        result.Should().BeTrue();
        context.Violations.Should().BeEmpty();
        context.RelevantAuthorizations.Should().ContainSingle();
    }

    [Theory]
    [InlineData(BigGameHuntingLicence.LicenceType.CanadianResident)]
    [InlineData(BigGameHuntingLicence.LicenceType.CanadianResidentSpecialGuided)]
    public async Task AuthorizationRule_WithCanadianResidentBigGameHuntingLicenceAndNoGuide_IsIllegal(
        BigGameHuntingLicence.LicenceType licenceType
    )
    {
        var dbContext = new AppDbContext();
        var dateOfDeath = new DateTimeOffset(2023, 6, 1, 0, 0, 0, TimeSpan.FromHours(-7));
        var validStart = dateOfDeath.AddDays(-10);
        var validEnd = dateOfDeath.AddDays(10);

        var report = new IndividualHuntedMortalityReport()
        {
            Person = new Client()
            {
                Authorizations = new()
                {
                    new BigGameHuntingLicence()
                    {
                        Season = new HuntingSeason(2023),
                        ValidFromDateTime = validStart,
                        ValidToDateTime = validEnd,
                        Type = licenceType
                    }
                }
            },
            HuntedActivity = new HuntedActivity()
            {
                Mortality = new AmericanBlackBearMortality() { DateOfDeath = dateOfDeath }
            }
        };

        var item = new BigGameHuntingLicenceRulePipelineItem();

        var context = new AuthorizationRulePipelineContext(dbContext);

        var result = await item.Process(report, context);

        result.Should().BeTrue();
        context.Violations.Should().ContainSingle();
        var violation = context.Violations.First();

        violation
            .Should()
            .BeEquivalentTo(
                new Violation(
                    report.GetActivities().First(),
                    RuleType.HuntedWithoutAGuideAsCanadianResident,
                    SeverityType.Illegal,
                    "Hunted big game as a Canadian resident without a guide."
                )
            );
        context.RelevantAuthorizations.Should().ContainSingle();
    }

    [Theory]
    [InlineData(BigGameHuntingLicence.LicenceType.CanadianResident)]
    [InlineData(BigGameHuntingLicence.LicenceType.CanadianResidentSpecialGuided)]
    public async Task AuthorizationRule_WithCanadianResidentBigGameHuntingLicenceAndGuided_IsLegal(
        BigGameHuntingLicence.LicenceType licenceType
    )
    {
        var dbContext = new AppDbContext();
        var dateOfDeath = new DateTimeOffset(2023, 6, 1, 0, 0, 0, TimeSpan.FromHours(-7));
        var validStart = dateOfDeath.AddDays(-10);
        var validEnd = dateOfDeath.AddDays(10);

        Report report = null;
        if (licenceType == BigGameHuntingLicence.LicenceType.CanadianResident)
        {
            report = new OutfitterGuidedHuntReport
            {
                Client = new Client()
                {
                    Authorizations = new()
                    {
                        new BigGameHuntingLicence()
                        {
                            Season = new HuntingSeason(2023),
                            ValidFromDateTime = validStart,
                            ValidToDateTime = validEnd,
                            Type = licenceType
                        }
                    }
                },
                HuntedActivities = new()
                {
                    new HuntedActivity()
                    {
                        Mortality = new AmericanBlackBearMortality() { DateOfDeath = dateOfDeath }
                    }
                }
            };
        }
        else if (licenceType == BigGameHuntingLicence.LicenceType.CanadianResidentSpecialGuided)
        {
            report = new SpecialGuidedHuntReport
            {
                Client = new Client()
                {
                    Authorizations = new()
                    {
                        new BigGameHuntingLicence()
                        {
                            Season = new HuntingSeason(2023),
                            ValidFromDateTime = validStart,
                            ValidToDateTime = validEnd,
                            Type = licenceType
                        }
                    }
                },
                HuntedActivities = new()
                {
                    new HuntedActivity()
                    {
                        Mortality = new AmericanBlackBearMortality() { DateOfDeath = dateOfDeath }
                    }
                }
            };
        }

        var item = new BigGameHuntingLicenceRulePipelineItem();

        var context = new AuthorizationRulePipelineContext(dbContext);

        var result = await item.Process(report!, context);

        result.Should().BeTrue();
        context.Violations.Should().BeEmpty();
        context.RelevantAuthorizations.Should().ContainSingle();
    }
}
