using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Data.Rules;

public class RuleResult
{
    public IEnumerable<Violation> Violations { get; private set; } = Array.Empty<Violation>();
    public IEnumerable<Authorization> Authorizations { get; private set; } =
        Array.Empty<Authorization>();

    public bool IsApplicable { get; private set; } = true;

    private RuleResult() { }

    public static RuleResult NotApplicable => new() { IsApplicable = false };
    public static RuleResult IsLegal => new();

    public static RuleResult IsIllegal(IEnumerable<Violation> violations) =>
        new() { Violations = violations };
}
