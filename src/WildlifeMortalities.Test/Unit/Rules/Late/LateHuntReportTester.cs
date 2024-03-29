﻿using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Entities.Seasons;
using WildlifeMortalities.Shared.Services.Rules.Late;
using WildlifeMortalities.Test.Helpers;

namespace WildlifeMortalities.Test.Unit.Rules.Late;

public class LateHuntReportTester
{
    private static readonly DateTimeOffset s_reportedSubmittedDate =
        new(2023, 6, 15, 0, 0, 0, TimeSpan.FromHours(-7));
    private static readonly DateTimeOffset s_3DaysPrevious = s_reportedSubmittedDate.AddDays(-3);
    private static readonly DateTimeOffset s_4DaysPrevious = s_reportedSubmittedDate.AddDays(-4);
    private static readonly DateTimeOffset s_15DaysPrevious = s_reportedSubmittedDate.AddDays(-15);
    private static readonly DateTimeOffset s_lastDayOf2PriorMonths =
        new(2023, 4, 30, 0, 0, 0, TimeSpan.FromHours(-7));

    [Theory]
    [MemberData(nameof(GetTestActivities))]
    public async Task Process_WithParameterizedInputs(
        HuntedActivity activity,
        bool shouldBeLate,
        DateTimeOffset? reportedSubmittedDate
    )
    {
        var report = new IndividualHuntedMortalityReport
        {
            Activity = activity,
            DateSubmitted = reportedSubmittedDate ?? s_reportedSubmittedDate
        };

        var context = TestDbContextFactory.CreateContext();
        var rule = new LateHuntReportRule();

        report.Season = new HuntingSeason(2023);
        context.Seasons.Add(report.Season);
        await context.SaveChangesAsync();

        var result = await rule.Process(report, context);

        if (shouldBeLate)
            result.Violations.Should().ContainSingle();
        else
        {
            result.Violations.Should().BeEmpty();
        }
    }

    public static IEnumerable<object[]> GetTestActivities()
    {
        return new List<object[]>
        {
            GenerateTestCase<MooseMortality>(s_3DaysPrevious, false, true),
            GenerateTestCase<MooseMortality>(s_4DaysPrevious, true, true),
            GenerateTestCase<MooseMortality>(s_15DaysPrevious, true, true),
            GenerateTestCase<MooseMortality>(s_3DaysPrevious, false),
            GenerateTestCase<MooseMortality>(s_15DaysPrevious, false),
            GenerateTestCase<MooseMortality>(s_lastDayOf2PriorMonths, true),
            GenerateTestCaseCaribou(s_3DaysPrevious, false, CaribouMortality.CaribouHerd.Fortymile),
            GenerateTestCaseCaribou(s_4DaysPrevious, true, CaribouMortality.CaribouHerd.Fortymile),
            GenerateTestCaseCaribou(s_15DaysPrevious, true, CaribouMortality.CaribouHerd.Fortymile),
            GenerateTestCaseCaribou(s_3DaysPrevious, false, CaribouMortality.CaribouHerd.Nelchina),
            GenerateTestCaseCaribou(s_4DaysPrevious, true, CaribouMortality.CaribouHerd.Nelchina),
            GenerateTestCaseCaribou(s_15DaysPrevious, true, CaribouMortality.CaribouHerd.Nelchina),
            GenerateTestCaseCaribou(s_3DaysPrevious, false, CaribouMortality.CaribouHerd.Tay),
            GenerateTestCaseCaribou(s_15DaysPrevious, false, CaribouMortality.CaribouHerd.Tay),
            GenerateTestCaseCaribou(
                s_lastDayOf2PriorMonths,
                true,
                CaribouMortality.CaribouHerd.Tay
            ),
            GenerateTestCase<WoodBisonMortality>(s_reportedSubmittedDate.AddDays(-10), false),
            GenerateTestCase<WoodBisonMortality>(s_reportedSubmittedDate.AddDays(-11), true),
            GenerateTestCase<ThinhornSheepMortality>(s_3DaysPrevious, false),
            GenerateTestCase<ThinhornSheepMortality>(s_15DaysPrevious, false),
            GenerateTestCase<ThinhornSheepMortality>(s_lastDayOf2PriorMonths, true),
            GenerateTestCase<MountainGoatMortality>(s_3DaysPrevious, false),
            GenerateTestCase<MountainGoatMortality>(s_15DaysPrevious, false),
            GenerateTestCase<MountainGoatMortality>(s_lastDayOf2PriorMonths, true),
            GenerateTestCase<MuleDeerMortality>(s_3DaysPrevious, false),
            GenerateTestCase<MuleDeerMortality>(s_15DaysPrevious, false),
            GenerateTestCase<MuleDeerMortality>(s_lastDayOf2PriorMonths, true),
            GenerateTestCase<ElkMortality>(s_3DaysPrevious, false),
            GenerateTestCase<ElkMortality>(s_15DaysPrevious, true),
            GenerateTestCase<ElkMortality>(s_lastDayOf2PriorMonths, true),
            GenerateTestCase<GrizzlyBearMortality>(s_3DaysPrevious, false),
            GenerateTestCase<GrizzlyBearMortality>(s_15DaysPrevious, false),
            GenerateTestCase<GrizzlyBearMortality>(s_lastDayOf2PriorMonths, true),
            GenerateTestCase<AmericanBlackBearMortality>(s_3DaysPrevious, false),
            GenerateTestCase<AmericanBlackBearMortality>(s_15DaysPrevious, false),
            GenerateTestCase<AmericanBlackBearMortality>(s_lastDayOf2PriorMonths, true),
            GenerateTestCase<CoyoteMortality>(s_3DaysPrevious, false),
            GenerateTestCase<CoyoteMortality>(s_15DaysPrevious, false),
            GenerateTestCase<CoyoteMortality>(s_lastDayOf2PriorMonths, true),
            GenerateTestCase<WolverineMortality>(s_3DaysPrevious, false),
            GenerateTestCase<WolverineMortality>(s_15DaysPrevious, false),
            GenerateTestCase<WolverineMortality>(s_lastDayOf2PriorMonths, true),
            GenerateTestCaseGreyWolf(
                new DateTimeOffset(2024, 3, 20, 0, 0, 0, TimeSpan.FromHours(-7)),
                new DateTimeOffset(2024, 3, 15, 0, 0, 0, TimeSpan.FromHours(-7)),
                false
            ),
            GenerateTestCaseGreyWolf(
                new DateTimeOffset(2024, 4, 15, 23, 59, 59, TimeSpan.FromHours(-7)),
                new DateTimeOffset(2024, 3, 15, 0, 0, 0, TimeSpan.FromHours(-7)),
                false
            ),
            GenerateTestCaseGreyWolf(
                new DateTimeOffset(2024, 4, 16, 0, 0, 0, TimeSpan.FromHours(-7)),
                new DateTimeOffset(2024, 3, 15, 0, 0, 0, TimeSpan.FromHours(-7)),
                true
            ),
        };
    }

    private static object[] GenerateTestCase<T>(
        DateTimeOffset dateToVerify,
        bool shouldBeLate,
        bool isThreshold = false
    )
        where T : Mortality, new()
    {
        return new object[]
        {
            GetHuntedActivity<T>(dateToVerify, isThreshold),
            shouldBeLate,
            null!
        };
    }

    private static object[] GenerateTestCaseCaribou(
        DateTimeOffset dateOfDeath,
        bool shouldBeLate,
        CaribouMortality.CaribouHerd herd
    )
    {
        var activity = GetHuntedActivity<CaribouMortality>(dateOfDeath);
        (activity.Mortality as CaribouMortality)!.LegalHerd = herd;
        return new object[] { activity, shouldBeLate, null! };
    }

    private static HuntedActivity GetHuntedActivity<T>(
        DateTimeOffset dateToVerify,
        bool isThreshold = false
    )
        where T : Mortality, new()
    {
        var season = new HuntingSeason(dateToVerify.Year);

        return new HuntedActivity()
        {
            Mortality = new T() { DateOfDeath = dateToVerify },
            ActivityQueueItem = new()
            {
                BagLimitEntry = new HuntingBagLimitEntry(
                    Array.Empty<GameManagementArea>(),
                    Data.Enums.Species.GreyWolf,
                    season,
                    season.StartDate,
                    season.EndDate,
                    int.MaxValue,
                    null,
                    isThreshold ? 5 : null
                )
            }
        };
    }

    private static object[] GenerateTestCaseGreyWolf(
        DateTimeOffset dateToVerify,
        DateTimeOffset dateOfDeath,
        bool shouldBeLate
    )
    {
        var season =
            dateOfDeath.Month > 3
                ? new HuntingSeason(dateOfDeath.Year)
                : new HuntingSeason(dateOfDeath.Year - 1);
        var activity = new HuntedActivity()
        {
            Mortality = new GreyWolfMortality() { DateOfDeath = dateOfDeath },
            ActivityQueueItem = new()
            {
                BagLimitEntry = new HuntingBagLimitEntry(
                    Array.Empty<GameManagementArea>(),
                    Data.Enums.Species.GreyWolf,
                    season,
                    season.StartDate,
                    season.EndDate,
                    int.MaxValue
                )
            }
        };
        return new object[] { activity, shouldBeLate, dateToVerify };
    }
}
