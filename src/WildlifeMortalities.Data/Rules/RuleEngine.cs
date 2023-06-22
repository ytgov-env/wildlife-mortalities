using WildlifeMortalities.Data.Rules.Authorizations;

namespace WildlifeMortalities.Data.Rules;

public static class RuleEngine
{
    public static IEnumerable<Rule> Rules { get; } =
        new List<Rule> { new BagLimitRule(), new AuthorizationRule(), new LateReportRule(), };
}
