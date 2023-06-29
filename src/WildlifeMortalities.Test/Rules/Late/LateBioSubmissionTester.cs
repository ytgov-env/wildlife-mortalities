using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Rules.Late;
using WildlifeMortalities.Test.Helpers;

namespace WildlifeMortalities.Test.Rules.Late;

public class LateBioSubmissionTester
{
    private static readonly DateTimeOffset s_bioSubmissionSubmittedDate =
        new(2023, 6, 15, 0, 0, 0, TimeSpan.FromHours(-7));
    private static readonly DateTimeOffset s_3DaysPrevious = s_bioSubmissionSubmittedDate.AddDays(
        -3
    );
    private static readonly DateTimeOffset s_4DaysPrevious = s_bioSubmissionSubmittedDate.AddDays(
        -4
    );
    private static readonly DateTimeOffset s_15DaysPrevious = s_bioSubmissionSubmittedDate.AddDays(
        -15
    );

    private static readonly DateTimeOffset s_16DaysPrevious = s_bioSubmissionSubmittedDate.AddDays(
        -16
    );
    private static readonly DateTimeOffset s_lastDayOfPreviousPreviousMonth =
        new(2023, 4, 30, 0, 0, 0, TimeSpan.FromHours(-7));

    [Theory]
    [MemberData(nameof(GetTestBioSubmissions))]
    public async Task Process_WithParameterizedInputs(
        HarvestActivity activity,
        BioSubmission bioSubmission,
        bool shouldBeLate
    )
    {
        Report report = null!;
        if (activity is HuntedActivity huntedActivity)
        {
            report = new IndividualHuntedMortalityReport { HuntedActivity = huntedActivity };
        }
        else if (activity is TrappedActivity trappedActivity)
        {
            report = new TrappedMortalitiesReport
            {
                TrappedActivities = new List<TrappedActivity> { trappedActivity }
            };
        }

        var rule = new LateBioSubmissionRule();
        var context = TestDbContextFactory.GetContext();
        context.BioSubmissions.Add(bioSubmission);
        await context.SaveChangesAsync();
        var result = await rule.Process(report, context);

        if (shouldBeLate)
        {
            result.Violations.Should().ContainSingle();
        }
        else
        {
            result.Violations.Should().BeEmpty();
        }
    }

    public static IEnumerable<object[]> GetTestBioSubmissions()
    {
        return new List<object[]>
        {
            #region hunting
            GenerateTestCaseCaribou(s_3DaysPrevious, false, CaribouMortality.CaribouHerd.Fortymile),
            GenerateTestCaseCaribou(s_4DaysPrevious, true, CaribouMortality.CaribouHerd.Fortymile),
            GenerateTestCaseCaribou(s_15DaysPrevious, true, CaribouMortality.CaribouHerd.Fortymile),
            GenerateTestCaseCaribou(s_3DaysPrevious, false, CaribouMortality.CaribouHerd.Nelchina),
            GenerateTestCaseCaribou(s_4DaysPrevious, true, CaribouMortality.CaribouHerd.Nelchina),
            GenerateTestCaseCaribou(s_15DaysPrevious, true, CaribouMortality.CaribouHerd.Nelchina),
            GenerateTestCase<WoodBisonMortality, WoodBisonBioSubmission, HuntedActivity>(
                s_3DaysPrevious,
                false
            ),
            GenerateTestCase<WoodBisonMortality, WoodBisonBioSubmission, HuntedActivity>(
                s_15DaysPrevious,
                false
            ),
            GenerateTestCase<WoodBisonMortality, WoodBisonBioSubmission, HuntedActivity>(
                s_lastDayOfPreviousPreviousMonth,
                true
            ),
            GenerateTestCase<ThinhornSheepMortality, ThinhornSheepBioSubmission, HuntedActivity>(
                s_3DaysPrevious,
                false
            ),
            GenerateTestCase<ThinhornSheepMortality, ThinhornSheepBioSubmission, HuntedActivity>(
                s_15DaysPrevious,
                false
            ),
            GenerateTestCase<ThinhornSheepMortality, ThinhornSheepBioSubmission, HuntedActivity>(
                s_lastDayOfPreviousPreviousMonth,
                true
            ),
            GenerateTestCase<MountainGoatMortality, MountainGoatBioSubmission, HuntedActivity>(
                s_3DaysPrevious,
                false
            ),
            GenerateTestCase<MountainGoatMortality, MountainGoatBioSubmission, HuntedActivity>(
                s_15DaysPrevious,
                false
            ),
            GenerateTestCase<MountainGoatMortality, MountainGoatBioSubmission, HuntedActivity>(
                s_lastDayOfPreviousPreviousMonth,
                true
            ),
            GenerateTestCase<MuleDeerMortality, MuleDeerBioSubmission, HuntedActivity>(
                s_3DaysPrevious,
                false
            ),
            GenerateTestCase<MuleDeerMortality, MuleDeerBioSubmission, HuntedActivity>(
                s_15DaysPrevious,
                false
            ),
            GenerateTestCase<MuleDeerMortality, MuleDeerBioSubmission, HuntedActivity>(
                s_lastDayOfPreviousPreviousMonth,
                true
            ),
            GenerateTestCase<ElkMortality, ElkBioSubmission, HuntedActivity>(
                s_3DaysPrevious,
                false
            ),
            GenerateTestCase<ElkMortality, ElkBioSubmission, HuntedActivity>(
                s_15DaysPrevious,
                true
            ),
            GenerateTestCase<ElkMortality, ElkBioSubmission, HuntedActivity>(
                s_lastDayOfPreviousPreviousMonth,
                true
            ),
            GenerateTestCase<GrizzlyBearMortality, GrizzlyBearBioSubmission, HuntedActivity>(
                s_3DaysPrevious,
                false
            ),
            GenerateTestCase<GrizzlyBearMortality, GrizzlyBearBioSubmission, HuntedActivity>(
                s_15DaysPrevious,
                false
            ),
            GenerateTestCase<GrizzlyBearMortality, GrizzlyBearBioSubmission, HuntedActivity>(
                s_lastDayOfPreviousPreviousMonth,
                true
            ),
            GenerateTestCase<
                AmericanBlackBearMortality,
                AmericanBlackBearBioSubmission,
                HuntedActivity
            >(s_3DaysPrevious, false),
            GenerateTestCase<
                AmericanBlackBearMortality,
                AmericanBlackBearBioSubmission,
                HuntedActivity
            >(s_15DaysPrevious, false),
            GenerateTestCase<
                AmericanBlackBearMortality,
                AmericanBlackBearBioSubmission,
                HuntedActivity
            >(s_lastDayOfPreviousPreviousMonth, true),
            GenerateTestCase<WolverineMortality, WolverineBioSubmission, HuntedActivity>(
                s_3DaysPrevious,
                false
            ),
            GenerateTestCase<WolverineMortality, WolverineBioSubmission, HuntedActivity>(
                s_15DaysPrevious,
                false
            ),
            GenerateTestCase<WolverineMortality, WolverineBioSubmission, HuntedActivity>(
                s_lastDayOfPreviousPreviousMonth,
                true
            ),
            //Todo: wolf
            #endregion

            #region trapping
            //Todo: wolf
            GenerateTestCase<WolverineMortality, WolverineBioSubmission, TrappedActivity>(
                s_3DaysPrevious,
                false
            ),
            GenerateTestCase<WolverineMortality, WolverineBioSubmission, TrappedActivity>(
                s_15DaysPrevious,
                false
            ),
            GenerateTestCase<WolverineMortality, WolverineBioSubmission, TrappedActivity>(
                s_16DaysPrevious,
                true
            ),
            GenerateTestCase<CanadaLynxMortality, CanadaLynxBioSubmission, TrappedActivity>(
                s_3DaysPrevious,
                false
            ),
            GenerateTestCase<CanadaLynxMortality, CanadaLynxBioSubmission, TrappedActivity>(
                s_15DaysPrevious,
                false
            ),
            GenerateTestCase<CanadaLynxMortality, CanadaLynxBioSubmission, TrappedActivity>(
                s_16DaysPrevious,
                true
            ),
            #endregion
        };
    }

    private static object[] GenerateTestCase<TMortality, TBioSubmission, TActivity>(
        DateTimeOffset deadlineDate,
        bool shouldBeLate
    )
        where TMortality : Mortality, new()
        where TBioSubmission : BioSubmission<TMortality>, new()
        where TActivity : HarvestActivity, new()
    {
        var bioSubmission = GetBioSubmission<TMortality, TBioSubmission, TActivity>(deadlineDate);
        return new object[] { bioSubmission.Mortality.Activity, bioSubmission, shouldBeLate };
    }

    private static object[] GenerateTestCaseCaribou(
        DateTimeOffset dateOfDeath,
        bool shouldBeLate,
        CaribouMortality.CaribouHerd herd
    )
    {
        var bioSubmission = GetBioSubmission<
            CaribouMortality,
            CaribouBioSubmission,
            HuntedActivity
        >(dateOfDeath);
        bioSubmission.Mortality.Herd = herd;
        return new object[] { bioSubmission.Mortality.Activity, bioSubmission, shouldBeLate };
    }

    private static BioSubmission<TMortality> GetBioSubmission<
        TMortality,
        TBioSubmission,
        TActivity
    >(DateTimeOffset deadlineDate)
        where TMortality : Mortality, new()
        where TBioSubmission : BioSubmission<TMortality>, new()
        where TActivity : HarvestActivity, new()
    {
        return new TBioSubmission()
        {
            DateSubmitted = s_bioSubmissionSubmittedDate,
            Mortality = new TMortality()
            {
                Sex = Data.Enums.Sex.Male,
                DateOfDeath = deadlineDate,
                Activity = new TActivity()
                {
                    ActivityQueueItem = new()
                    {
                        BagLimitEntry =
                            typeof(TActivity) == typeof(HuntedActivity)
                                ? new HuntingBagLimitEntry()
                                : new TrappingBagLimitEntry() { PeriodEnd = deadlineDate }
                    }
                }
            }
        };
    }
}
