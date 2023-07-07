using System;
using System.IO;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Rules.BioSubmissions;
using WildlifeMortalities.Test.Helpers;
using Xunit;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Test.Rules.BioSubmissions;

public class SheepBioSubmissionTester
{
    //public static IEnumerable<object[]> GetAllNonSheepMortalityTypes() =>
    //    typeof(Mortality).GetAllDerivedTypesExcludingSomeTypes(
    //        new Type[] { typeof(ThinhornSheepMortality) }
    //    );

    //[Theory]
    //[MemberData(nameof(GetAllNonSheepMortalityTypes))]
    //public async Task Process_WithNonSheepMortality_ReturnsNotApplicable(object type)
    //{
    //    var mortality = Activator.CreateInstance((Type)type) as Mortality;
    //    var report = new IndividualHuntedMortalityReport()
    //    {
    //        HuntedActivity = new() { Mortality = mortality! }
    //    };

    //    // Todo: create bio submission

    //    var context = TestDbContextFactory.CreateContext();
    //    var rule = new SheepBioSubmissionRule();

    //    var result = await rule.Process(report, context);
    //    result.IsApplicable.Should().BeFalse();
    //    result.Violations.Should().BeEmpty();
    //}

    [Fact]
    public async Task Process_WithCompleteEyeSocketsAnd8YearsOld_ReturnsNoViolations()
    {
        var bioSubmission = new ThinhornSheepBioSubmission()
        {
            IsHornsProvided = true,
            IsHeadProvided = true,
            IsBothEyeSocketsComplete = true,
            IsFullCurl = false,
            Age = new() { Years = 8, Confidence = ConfidenceInAge.Good }
        };
        bioSubmission.UpdateAnalysisStatus();
        var report = new IndividualHuntedMortalityReport()
        {
            HuntedActivity = new()
            {
                Mortality = new ThinhornSheepMortality()
                {
                    BioSubmission = bioSubmission,
                    Sex = Data.Enums.Sex.Male
                }
            }
        };
        var context = TestDbContextFactory.CreateContext();
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        var rule = new SheepBioSubmissionRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithIncompleteEyeSockets_ReturnsViolation()
    {
        var bioSubmission = new ThinhornSheepBioSubmission()
        {
            IsHornsProvided = true,
            IsHeadProvided = true,
            IsBothEyeSocketsComplete = false,
            Age = new() { Years = 10, Confidence = ConfidenceInAge.Good }
        };
        bioSubmission.UpdateAnalysisStatus();
        var report = new IndividualHuntedMortalityReport()
        {
            HuntedActivity = new()
            {
                Mortality = new ThinhornSheepMortality()
                {
                    BioSubmission = bioSubmission,
                    Sex = Data.Enums.Sex.Male
                }
            }
        };
        var context = TestDbContextFactory.CreateContext();
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        var rule = new SheepBioSubmissionRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().ContainSingle();
        var violation = result.Violations.First();

        violation
            .Should()
            .BeEquivalentTo(
                new Violation(
                    report.GetActivities().First(),
                    RuleType.SheepEyeSocketIncomplete,
                    SeverityType.PotentiallyIllegal,
                    "Sheep has incomplete eye socket(s)."
                )
            );
    }

    [Fact]
    public async Task Process_Under8AndNotFullCurl_ReturnsViolation()
    {
        var bioSubmission = new ThinhornSheepBioSubmission()
        {
            IsHornsProvided = true,
            IsHeadProvided = true,
            IsBothEyeSocketsComplete = true,
            IsFullCurl = false,
            Age = new() { Years = 7, Confidence = ConfidenceInAge.Good }
        };
        bioSubmission.UpdateAnalysisStatus();
        var report = new IndividualHuntedMortalityReport()
        {
            HuntedActivity = new()
            {
                Mortality = new ThinhornSheepMortality()
                {
                    BioSubmission = bioSubmission,
                    Sex = Data.Enums.Sex.Male
                }
            }
        };
        var context = TestDbContextFactory.CreateContext();
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        var rule = new SheepBioSubmissionRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeTrue();
        result.Violations.Should().ContainSingle();
        var violation = result.Violations.First();

        violation
            .Should()
            .BeEquivalentTo(
                new Violation(
                    report.GetActivities().First(),
                    RuleType.SheepYoungerThan8AndNotFullCurl,
                    SeverityType.Illegal,
                    "Sheep is under 8 years old and not full curl."
                )
            );
    }
}
