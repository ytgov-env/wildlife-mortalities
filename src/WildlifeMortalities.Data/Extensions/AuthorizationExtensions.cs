using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Extensions;

public static class AuthorizationExtensions
{
    public static T? GetValidAuthorization<T>(
        this IEnumerable<Authorization> authorizations,
        Activity activity
    )
        where T : Authorization
    {
        if (typeof(T) == typeof(HuntingSeal))
        {
            throw new InvalidOperationException(
                "This method does not support hunting seals, as they cannot be associated automatically. The user must manually select a hunting seal to associate with an activity."
            );
        }
        return authorizations
            .OfType<T>()
            .SingleOrDefault(
                x =>
                    x.ValidFromDateTime <= activity.Mortality.DateOfDeath
                    && x.ValidToDateTime >= activity.Mortality.DateOfDeath
            );
    }
}
