using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Entities;

public class Violation
{
    private Violation() { }

    public Violation(Activity activity, RuleType rule, SeverityType severity, string description)
    {
        Activity = activity ?? throw new ArgumentNullException(nameof(activity));

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                $"'{nameof(description)}' cannot be null or whitespace.",
                nameof(description)
            );
        }

        Rule = rule;
        Severity = severity;
        Description = description;
    }

    public int Id { get; set; }
    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;
    public RuleType Rule { get; init; }
    public SeverityType Severity { get; init; }
    public string Description { get; set; } = string.Empty;

    // 0-99 Harvest periods, bag limits & thresholds
    // 100-199 Authorizations
    // 200-299 Biological Submissions
    // 300-399 Late
    // 400-499 Other
    public enum RuleType
    {
        HarvestPeriod = 10,
        BagLimitExceeded = 20,
        ThresholdExceeded = 30,
        KilledMoreThanOneGrizzlyBearInThreeYearSpan = 40,
        Authorization = 100,
        NoValidBigGameHuntingLicence = 110,
        SpecialGuidedWithoutValidLicence = 120,
        HuntedWithoutAGuideAsCanadianResident = 130,
        AllRequiredSamplesNotSubmitted = 200,
        SomeRequiredSamplesNotSubmitted = 210,
        SheepEyeSocketIncomplete = 220,
        SheepYoungerThan8AndNotFullCurl = 230,
        KilledBearCub = 240,
        LateHuntReport = 300,
        LateBioSubmission = 310
    }

    public enum SeverityType
    {
        InternalError = 10,
        PotentiallyIllegal = 20,
        Illegal = 30
    }
}
