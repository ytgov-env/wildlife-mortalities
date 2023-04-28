using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public abstract class Authorization
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTimeOffset ValidFromDateTime { get; set; }
    public DateTimeOffset ValidToDateTime { get; set; }
    public DateTimeOffset LastModifiedDateTime { get; set; }
    public int SeasonId { get; set; }
    public Season Season { get; set; } = null!;

    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public static AuthorizationsSummary GetSummary(
        IEnumerable<Authorization> authorizations,
        Report report
    )
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

    public abstract AuthorizationResult GetResult(Report report);

    public record AuthorizationsSummary(
        IEnumerable<AuthorizationResult> ApplicableAuthorizationResults
    );

    public abstract string GetAuthorizationType();
}

public class AuthorizationConfig : IEntityTypeConfiguration<Authorization>
{
    public void Configure(EntityTypeBuilder<Authorization> builder) { }
}
