using WildlifeMortalities.Data.Rules.Authorizations;
using WildlifeMortalities.Data.Rules.BioSubmissions;
using WildlifeMortalities.Data.Rules.Late;

namespace WildlifeMortalities.Data.Rules;

public static class RulesEngine
{
    // The rules in this collection are executed sequentially
    // BagLimitRule must run before LateHuntReportRule
    public static IEnumerable<Rule> Rules { get; } =
        new List<Rule>
        {
            new BagLimitRule(),
            new AuthorizationRule(),
            new LateHuntReportRule(),
            new LateBioSubmissionRule(),
            new MissingBioSubmissionRule(),
            new SheepBioSubmissionRule(),
        };
}
