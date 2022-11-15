using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class AuthorizationResult
{
    private AuthorizationResult(Authorization authorization)
    {
        this.Authorization = authorization;
    }

    public Authorization Authorization { get; }
    public bool IsApplicable { get; private set; }
    public bool ViolationFound { get; private set; }
    public string Message { get; private set; }

    public void SetAsNotApplicable() =>
        IsApplicable = true;

    public static AuthorizationResult NotApplicable(Authorization authorization) =>
        new AuthorizationResult(authorization) { IsApplicable = false };

    public static AuthorizationResult Forbidden(Authorization authorization, string message) =>
        new AuthorizationResult(authorization) { IsApplicable = true, ViolationFound = true, Message = message };

    public static AuthorizationResult Allowed(Authorization authorization)
    {
        throw new NotImplementedException();
    }
}

public abstract class Authorization
{
    public static (bool, IEnumerable<AuthorizationResult>) IsValid(IEnumerable<Authorization> authorizations,
        MortalityReport report)
    {
        bool? validAuthorizationFound = null;
        List<AuthorizationResult> applicableAuthorizationResults = new();
        foreach (var authorization in authorizations)
        {
            var authorizationResult = authorization.IsValid(report);
            if (authorizationResult.IsApplicable)
            {
                applicableAuthorizationResults.Add(authorizationResult);
            }

            if (authorizationResult.ViolationFound)
            {
                validAuthorizationFound = false;
            }
            else
            {
                // Todo Remove this if statement?
                // if (validAuthorizationFound == false)
                // {
                if (authorizationResult.IsApplicable == true && authorizationResult.ViolationFound == false)
                {
                    validAuthorizationFound = true;
                }
                // }
            }
        }

        return (validAuthorizationFound ?? false, applicableAuthorizationResults);
    }


    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime ValidFromDate { get; set; }
    public DateTime ValidToDate { get; set; }
    public string Season => $"{ValidFromDate.Year}-{ValidToDate.Year}";
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public abstract AuthorizationResult IsValid(MortalityReport report);
}

public class AuthorizationConfig : IEntityTypeConfiguration<Authorization>
{
    public void Configure(EntityTypeBuilder<Authorization> builder)
    {
    }
}
