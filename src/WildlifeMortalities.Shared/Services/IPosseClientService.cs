using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Shared.Services;

public interface IPosseClientService
{
    Task<
        IEnumerable<(string envClientId, Authorization, string? specialGuidedHunterEnvClientId)>
    > RetrieveData(DateTimeOffset modifiedSinceDateTime);
}
