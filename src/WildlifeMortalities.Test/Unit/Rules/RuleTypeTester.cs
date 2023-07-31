using WildlifeMortalities.Shared.Services.Rules;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Test.Unit.Rules;

public class RuleTypeTester
{
    [Fact]
    public void OnlyApplicableInOneRule()
    {
        var typesUsed = new List<RuleType>();
        foreach (var rule in RulesEngine.Rules)
        {
            var typesForRule = rule.ApplicableRuleTypes;
            typesUsed.AddRange(typesForRule);

            typesUsed.Distinct().Count().Should().Be(typesUsed.Count);
        }
    }
}
