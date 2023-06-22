using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Rules.Authorizations;

internal class AuthorizationRule : Rule
{
    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        var pipelineContext = new AuthorizationRulePipelineContext(context);
        var pipeline = new AuthorizationRulePipeline(pipelineContext);

        await pipeline.Process(report);

        if (pipelineContext.Violations.Any())
        {
            return RuleResult.IsIllegal(pipelineContext.Violations);
        }
        else
        {
            return pipelineContext.UsedAuthorizations.Any()
                ? RuleResult.IsLegal
                : RuleResult.NotApplicable;
        }
    }
}
