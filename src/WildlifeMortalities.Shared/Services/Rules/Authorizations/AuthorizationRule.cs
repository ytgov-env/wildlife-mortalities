using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Reports;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Shared.Services.Rules.Authorizations;

public class AuthorizationRule : Rule
{
    private Func<AuthorizationRulePipelineContext, AuthorizationRulePipeline> _pipelineFactory;

    public override IEnumerable<RuleType> ApplicableRuleTypes =>
        Enum.GetValues<RuleType>().Where(x => (int)x >= 100 && (int)x <= 199);

    internal AuthorizationRule(
        Func<AuthorizationRulePipelineContext, AuthorizationRulePipeline> pipelineFactory
    )
    {
        _pipelineFactory = pipelineFactory;
    }

    public AuthorizationRule()
        : this((x) => new AuthorizationRulePipeline(x)) { }

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        var pipelineContext = new AuthorizationRulePipelineContext(context);
        var pipeline = _pipelineFactory.Invoke(pipelineContext);

        await pipeline.Process(report);

        if (pipelineContext.Violations.Any())
        {
            return RuleResult.IsIllegal(pipelineContext.Violations);
        }
        else
        {
            return pipelineContext.RelevantAuthorizations.Any()
                ? RuleResult.IsLegal
                : RuleResult.NotApplicable;
        }
    }
}
