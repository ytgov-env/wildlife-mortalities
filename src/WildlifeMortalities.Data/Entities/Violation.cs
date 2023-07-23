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
    public Activity Activity { get; set; } = null!;
    public RuleType Rule { get; init; }
    public SeverityType Severity { get; init; }
    public string Description { get; set; } = string.Empty;

    // 0-99 Bag Limits
    // 100-199 Authorizations
    // 200-299 Biological Submissions
    // 300-399 Late
    // 400-499 Other
    public enum RuleType
    {
        BagLimitExceeded = 10,
        KilledMoreThanOneGrizzlyBearInThreeYearSpan = 20,
        Authorization = 100,
        NoValidBigGameHuntingLicence = 110,
        SpecialGuidedWithoutValidLicence = 120,
        HuntedWithoutAGuideAsCanadianResident = 130,
        AllRequiredSamplesNotSubmitted = 200,
        SomeRequiredSamplesNotSubmitted = 210,
        SheepEyeSocketIncomplete = 220,
        SheepYoungerThan8AndNotFullCurl = 230,
        BearCub = 240,
        LateReport = 300,
        LateBioSubmission = 310,
        HarvestPeriod = 400,
    }

    public enum SeverityType
    {
        InternalError = 10,
        PotentiallyIllegal = 20,
        Illegal = 30
    }
}
