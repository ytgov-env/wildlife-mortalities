using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Rules.Authorizations;

public class IndigenousAuthorizationRulePipelineItem : AuthorizationRulePipelineItem
{
    public override Task<bool> Process(Report report, AuthorizationRulePipelineContext context)
    {
        throw new NotImplementedException();
    }
}
