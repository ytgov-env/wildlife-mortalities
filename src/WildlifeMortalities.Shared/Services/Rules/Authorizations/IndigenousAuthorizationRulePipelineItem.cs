using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Shared.Services.Rules.Authorizations;

public class IndigenousAuthorizationRulePipelineItem : AuthorizationRulePipelineItem
{
    public override Task<bool> Process(Report report, AuthorizationRulePipelineContext context)
    {
        //Todo: complete
        return Task.FromResult(true);
    }
}
