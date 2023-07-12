using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Rules;

namespace WildlifeMortalities.Data.Entities;

public class RulesSummary
{
    public Report Report { get; set; } = null!;
    public List<Violation> Violations { get; set; } = null!;
    public List<Authorization> Authorizations { get; set; } = null!;

    public static async Task<RulesSummary> Generate(Report report, AppDbContext context)
    {
        var rulesSummary = new RulesSummary
        {
            Violations = new List<Violation>(),
            Authorizations = new List<Authorization>(),
            Report = report,
        };

        foreach (var item in RulesEngine.Rules)
        {
            var result = await item.Process(report, context);
            rulesSummary.Violations.AddRange(result.Violations);
            rulesSummary.Authorizations.AddRange(result.Authorizations);
        }

        return rulesSummary;
    }
}

//public class RulesSummaryConfig : IEntityTypeConfiguration<RulesSummary>
//{
//    public void Configure(EntityTypeBuilder<RulesSummary> builder)
//    {
//        builder.ToTable(Constants.TableNameConstants.RulesSummaries);
//        builder
//            .HasMany(x => x.Authorizations)
//            .WithMany(x => x.RulesSummaries)
//            .UsingEntity(
//                Constants.TableNameConstants.AuthorizationRulesSummary,
//                l =>
//                    l.HasOne(typeof(RulesSummary))
//                        .WithMany()
//                        .OnDelete(DeleteBehavior.ClientCascade)
//                        .HasForeignKey("RulesSummariesId")
//                        .HasPrincipalKey(nameof(RulesSummary.Id)),
//                r =>
//                    r.HasOne(typeof(Authorization))
//                        .WithMany()
//                        .OnDelete(DeleteBehavior.ClientCascade)
//                        .HasForeignKey("AuthorizationsId")
//                        .HasPrincipalKey(nameof(Authorization.Id)),
//                j => j.HasKey("RulesSummariesId", "AuthorizationsId")
//            );
//    }
//}
