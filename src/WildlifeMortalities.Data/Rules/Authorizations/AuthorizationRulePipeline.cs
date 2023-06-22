using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Rules.Authorizations;

public class AuthorizationRulePipeline
{
    public IEnumerable<AuthorizationRulePipelineItem> Items { get; set; } =
        new[] { new IndigenousAuthorizationRulePipelineItem() };

    private AuthorizationRulePipelineContext _context;

    public AuthorizationRulePipeline(AuthorizationRulePipelineContext context)
    {
        _context = context;
    }

    public async Task Process(Report report)
    {
        foreach (var item in Items)
        {
            var next = await item.Process(report, _context);
            if (!next)
                break;
        }
    }
}

public class AuthorizationRulePipelineContext
{
    public AuthorizationRulePipelineContext(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public ICollection<Violation> Violations { get; } = new List<Violation>();
    public ICollection<Authorization> UsedAuthorizations { get; } = new List<Authorization>();
    public AppDbContext DbContext { get; }
}
