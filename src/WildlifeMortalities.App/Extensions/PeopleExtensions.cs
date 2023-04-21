using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Extensions;

public static class PeopleExtensions
{
    public static IQueryable<Client> SearchByEnvClientIdOrLastName(
        this IQueryable<Client> clients,
        string searchTerm
    )
    {
        return clients
            .Where(c => c.LastName.StartsWith(searchTerm) || c.EnvClientId.StartsWith(searchTerm))
            .OrderBy(x => x.LastName);
    }

    public static IQueryable<ConservationOfficer> SearchByBadgeNumberOrLastName(
        this IQueryable<ConservationOfficer> conservationOfficers,
        string searchTerm
    )
    {
        return conservationOfficers
            .Where(c => c.LastName.StartsWith(searchTerm) || c.BadgeNumber.StartsWith(searchTerm))
            .OrderBy(x => x.LastName);
    }
}
