using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Shared.Services.Rules.Authorizations;

public class AuthorizationRulePipeline
{
    public IEnumerable<AuthorizationRulePipelineItem> Items { get; protected set; } =
        new AuthorizationRulePipelineItem[]
        {
            new IndigenousAuthorizationRulePipelineItem(),
            new BigGameHuntingLicenceRulePipelineItem()
        };

    private readonly AuthorizationRulePipelineContext _context;

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
    public ICollection<Authorization> RelevantAuthorizations { get; } = new List<Authorization>();
    public AppDbContext DbContext { get; }
}
