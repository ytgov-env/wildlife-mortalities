using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Rules.Authorizations;

public abstract class AuthorizationRulePipelineItem
{
    public abstract Task<bool> Process(
        Report report,
        AuthorizationRulePipelineContext pipelineContext
    );
}
