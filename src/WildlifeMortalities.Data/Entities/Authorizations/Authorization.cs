using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class AuthorizationResult
{
    public AuthorizationResult(Authorization authorization, List<Violation> violations, bool isApplicable = true)
    {
        Authorization = authorization;
        IsApplicable = isApplicable;
    }

    public Authorization Authorization { get; }
    public List<Violation> Violations { get; } = new();
    public bool HasViolations => Violations.Any();
    public bool IsApplicable { get; }

    public static AuthorizationResult NotApplicable(Authorization authorization) =>
        new(authorization, false);
}

public record AuthorizationsSummary(IEnumerable<AuthorizationResult> ApplicableAuthorizationResults);

public abstract class Authorization
{
    public static AuthorizationsSummary GetSummary(IEnumerable<Authorization> authorizations,
        MortalityReport report)
    {
        List<AuthorizationResult> applicableAuthorizationResults = new();
        foreach (var authorization in authorizations)
        {
            var authorizationResult = authorization.GetResult(report);
            if (authorizationResult.IsApplicable)
            {
                applicableAuthorizationResults.Add(authorizationResult);
            }
        }

        return new AuthorizationsSummary(applicableAuthorizationResults);
    }


    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime ValidFromDate { get; set; }
    public DateTime ValidToDate { get; set; }
    public string Season => $"{ValidFromDate.Year}-{ValidToDate.Year}";
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public abstract AuthorizationResult GetResult(MortalityReport report);
}

public class AuthorizationConfig : IEntityTypeConfiguration<Authorization>
{
    public void Configure(EntityTypeBuilder<Authorization> builder)
    {
    }
}
