using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Shared.Services;

public interface IPosseService
{
    Task<
        IEnumerable<(
            string envClientId,
            Authorization authorization,
            string? specialGuidedHunterEnvClientId
        )>
    > RetrieveAuthorizationData(DateTimeOffset modifiedSinceDateTime);
    Task<IEnumerable<Client>> RetrieveClientData(
        Dictionary<string, Client> clientMapper,
        DateTimeOffset modifiedSinceDateTime
    );
}
