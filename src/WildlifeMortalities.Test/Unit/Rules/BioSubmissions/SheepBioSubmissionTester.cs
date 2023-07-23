using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services.Rules.BioSubmissions;
using WildlifeMortalities.Test.Helpers;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Test.Unit.Rules.BioSubmissions;

public class SheepBioSubmissionTester
{
    public static IEnumerable<object[]> GetAllNonSheepMortalityTypes() =>
        typeof(Mortality).GetAllDerivedTypesExcludingSomeTypes(
            new Type[] { typeof(ThinhornSheepMortality) }
        );

    [Theory]
    [MemberData(nameof(GetAllNonSheepMortalityTypes))]
    public async Task Process_WithNonSheepMortality_ReturnsNotApplicable(object type1)
    {
        var context = TestDbContextFactory.CreateContext();

        var type = (Type)type1;
        var mortality = Activator.CreateInstance(type) as Mortality;
        mortality!.Sex = Data.Enums.Sex.Female;
        var report = new IndividualHuntedMortalityReport()
        {
            Activity = new() { Mortality = mortality! }
        };
        var abstractBioSubmissionType = typeof(BioSubmission<>).MakeGenericType(new[] { type });

        var types = abstractBioSubmissionType.GetAllDerivedTypes();
        if (types.Any())
        {
            var bioSubmissionConstructor = types.First().GetConstructor(new Type[] { type });
            var instance =
                bioSubmissionConstructor.Invoke(new object[] { mortality }) as BioSubmission;

            context.Mortalities.Add(mortality);
            context.BioSubmissions.Add(instance!);
            context.SaveChanges();
        }

        // Todo: create bio submission

        var rule = new SheepBioSubmissionRule();

        var result = await rule.Process(report, context);
        result.IsApplicable.Should().BeFalse();
        result.Violations.Should().BeEmpty();
    }

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
            Activity = new()
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
            Activity = new()
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
            Activity = new()
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
