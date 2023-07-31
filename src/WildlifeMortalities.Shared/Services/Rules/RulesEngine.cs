using WildlifeMortalities.Shared.Services.Rules.Authorizations;
using WildlifeMortalities.Shared.Services.Rules.BioSubmissions;
using WildlifeMortalities.Shared.Services.Rules.Late;

namespace WildlifeMortalities.Shared.Services.Rules;

public static class RulesEngine
{
    // The rules in this collection are executed sequentially
    // BagLimitRule must run before LateHuntReportRule
    public static IEnumerable<Rule> Rules { get; } =
        new List<Rule>
        {
            new BagLimitRule(),
            new ThresholdRule(),
            //new AuthorizationRule(),
            new LateHuntReportRule(),
            new LateBioSubmissionRule(),
            new MissingBioSubmissionRule(),
            new SheepBioSubmissionRule(),
            new BearBioSubmissionRule(),
            //new MaxOneGrizzlyBearEveryThreeYearsRule(),
        };

    public static IEnumerable<Rule> PeriodicRules { get; } =
        new List<Rule> { new LateHuntReportRule(), new LateBioSubmissionRule(), };
}
