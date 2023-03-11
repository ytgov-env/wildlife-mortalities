using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Shared.Services;

public interface IPosseService
{
    Task<IEnumerable<(Authorization, string)>> RetrieveAuthorizationData(
        DateTimeOffset modifiedSinceDateTime
    );
    Task<IEnumerable<(Client, IEnumerable<string>)>> RetrieveClientData(
        DateTimeOffset modifiedSinceDateTime
    );
}
