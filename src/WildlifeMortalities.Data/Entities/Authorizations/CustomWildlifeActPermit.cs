using System.ComponentModel.DataAnnotations.Schema;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class CustomWildlifeActPermit : Authorization
{
    [Column($"{nameof(CustomWildlifeActPermit)}_{nameof(Conditions)}")]
    public string Conditions { get; set; } = string.Empty;

    public override string GetAuthorizationType() => "Custom wildlife act permit";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not CustomWildlifeActPermit customWildlifeActPermit)
        {
            throw new ArgumentException(
                $"Expected {nameof(CustomWildlifeActPermit)} but received {authorization.GetType().Name}"
            );
        }

        Conditions = customWildlifeActPermit.Conditions;
    }
}
