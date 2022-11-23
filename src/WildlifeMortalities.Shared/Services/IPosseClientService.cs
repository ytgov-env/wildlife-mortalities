using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Shared.Services;

public interface IPosseClientService
{
    Task<IEnumerable<(string, Authorization)>> RetrieveData(DateTimeOffset modifiedSinceDateTime);
}
