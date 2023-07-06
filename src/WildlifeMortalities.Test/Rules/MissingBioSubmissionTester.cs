using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Test.Helpers;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using static WildlifeMortalities.Data.Entities.Violation;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Rules.BioSubmissions;

namespace WildlifeMortalities.Test.Rules;

public class MissingBioSubmissionTester
{
    [Fact]
    public async Task Process_WithMortalityThatDoesntRequireABioSubmission_ReturnsNotApplicable()
    {
        var report = new IndividualHuntedMortalityReport()
        {
            HuntedActivity = new() { Mortality = new CoyoteMortality() }
        };
        var context = TestDbContextFactory.CreateContext();
        var rule = new MissingBioSubmissionRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeFalse();
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithMortalityRequiresBioSubmissionAndWasPartiallySubmitted_ReturnsViolation()
    {
        var biosubmission = new GrizzlyBearBioSubmission
        {
            IsEvidenceOfSexAttached = true,
            IsSkullProvided = false
        };
        biosubmission.UpdateRequiredOrganicMaterialsStatus();

        var report = new IndividualHuntedMortalityReport()
        {
            HuntedActivity = new()
            {
                Mortality = new GrizzlyBearMortality()
                {
                    BioSubmission = biosubmission,
                    Sex = Data.Enums.Sex.Male
                }
            }
        };
        var context = TestDbContextFactory.CreateContext();
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        var rule = new MissingBioSubmissionRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().ContainSingle();
        var violation = result.Violations.First();

        violation
            .Should()
            .BeEquivalentTo(
                new Violation(
                    report.GetActivities().First(),
                    RuleType.SomeRequiredSamplesNotSubmitted,
                    SeverityType.Illegal,
                    "Some of the required samples for grizzly bear were not submitted."
                )
            );
    }

    [Fact]
    public async Task Process_WithMortalityRequiresBioSubmissionAndWasNotSubmitted_ReturnsViolation()
    {
        var biosubmission = new GrizzlyBearBioSubmission
        {
            IsEvidenceOfSexAttached = false,
            IsSkullProvided = false
        };
        biosubmission.UpdateRequiredOrganicMaterialsStatus();

        var report = new IndividualHuntedMortalityReport()
        {
            HuntedActivity = new()
            {
                Mortality = new GrizzlyBearMortality()
                {
                    BioSubmission = biosubmission,
                    Sex = Data.Enums.Sex.Male
                }
            }
        };
        var context = TestDbContextFactory.CreateContext();
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        var rule = new MissingBioSubmissionRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().ContainSingle();
        var violation = result.Violations.First();

        violation
            .Should()
            .BeEquivalentTo(
                new Violation(
                    report.GetActivities().First(),
                    RuleType.AllRequiredSamplesNotSubmitted,
                    SeverityType.Illegal,
                    "All of the required samples for grizzly bear were not submitted."
                )
            );
    }

    [Fact]
    public async Task Process_WithMortalityRequiresBioSubmissionAndWasSubmitted_ReturnsNoViolation()
    {
        var biosubmission = new GrizzlyBearBioSubmission
        {
            IsEvidenceOfSexAttached = true,
            IsSkullProvided = true
        };
        biosubmission.UpdateRequiredOrganicMaterialsStatus();

        var report = new IndividualHuntedMortalityReport()
        {
            HuntedActivity = new()
            {
                Mortality = new GrizzlyBearMortality()
                {
                    BioSubmission = biosubmission,
                    Sex = Data.Enums.Sex.Male
                }
            }
        };
        var context = TestDbContextFactory.CreateContext();
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        var rule = new MissingBioSubmissionRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithMortalityRequiresBioSubmissionAndWasNotStarted_ReturnsNoViolation()
    {
        var report = new IndividualHuntedMortalityReport()
        {
            HuntedActivity = new()
            {
                Mortality = new GrizzlyBearMortality()
                {
                    BioSubmission = new GrizzlyBearBioSubmission(),
                    Sex = Data.Enums.Sex.Male
                }
            }
        };
        var context = TestDbContextFactory.CreateContext();
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        var rule = new MissingBioSubmissionRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().BeEmpty();
    }
}
