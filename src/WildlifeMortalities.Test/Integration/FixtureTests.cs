using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities;
using static WildlifeMortalities.Data.Entities.Violation;
using WildlifeMortalities.Shared.Services.Rules.BioSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.DataSeeder;

namespace WildlifeMortalities.Test.Integration;

public class FixtureTests : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture { get; }

    public FixtureTests(TestDatabaseFixture fixture) => Fixture = fixture;

    //[Fact]
    //public async Task GetMortality()
    //{
    //    using var context = Fixture.CreateContext();
    //    await Seeder.Seed(context);
    //    var biosubmission = new GrizzlyBearBioSubmission
    //    {
    //        IsEvidenceOfSexAttached = true,
    //        IsSkullProvided = false
    //    };
    //    biosubmission.UpdateRequiredOrganicMaterialsStatus();

    //    var report = new IndividualHuntedMortalityReport()
    //    {
    //        HuntedActivity = new()
    //        {
    //            Mortality = new GrizzlyBearMortality()
    //            {
    //                BioSubmission = biosubmission,
    //                Sex = Data.Enums.Sex.Male
    //            }
    //        }
    //    };
    //    context.Reports.Add(report);
    //    await context.SaveChangesAsync();

    //    var rule = new MissingBioSubmissionRule();

    //    var result = await rule.Process(report, context);
    //    result.IsApplicable.Should().BeTrue();
    //    result.Violations.Should().ContainSingle();
    //    var violation = result.Violations.First();

    //    violation
    //        .Should()
    //        .BeEquivalentTo(
    //            new Violation(
    //                report.GetActivities().First(),
    //                RuleType.SomeRequiredSamplesNotSubmitted,
    //                SeverityType.Illegal,
    //                "Some of the required samples for grizzly bear were not submitted."
    //            )
    //        );
    //    context.ChangeTracker.Clear();
    //}
}
