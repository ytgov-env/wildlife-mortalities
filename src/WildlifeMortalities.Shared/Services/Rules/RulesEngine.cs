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
            //new AuthorizationRule(),
            new LateHuntReportRule(),
            new LateBioSubmissionRule(),
            new MissingBioSubmissionRule(),
            // Todo: need to run this rule when the bio submission is updated, not just when the report is updated
            new SheepBioSubmissionRule(),
            new BearBioSubmissionRule(),
            //new MaxOneGrizzlyBearEveryThreeYearsRule(),
        };
}
