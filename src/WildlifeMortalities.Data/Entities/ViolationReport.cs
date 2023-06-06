using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Rules;

namespace WildlifeMortalities.Data.Entities;

public class ViolationReport
{
    public int Id { get; set; }
    public Report Report { get; set; } = null!;
    public List<Violation> Violations { get; set; } = null!;
    public List<Authorization> Authorizations { get; set; } = null!;

    public static async Task<ViolationReport> Generate(Report report, AppDbContext context)
    {
        var violationReport = new ViolationReport();
        var rules = RuleEngine.Rules;
        foreach (var item in rules)
        {
            var result = await item.Process(report, context);
            violationReport.Violations.AddRange(result.Violations);
            violationReport.Authorizations.AddRange(result.Authorizations);
        }

        return violationReport;
    }
}
