using WildlifeMortalities.Data.Entities;

namespace WildlifeMortalities.Data.Extensions;

public static class RegisteredTrappingConcessionExtensions
{
    public static string ConcessionsToString(
        this IEnumerable<RegisteredTrappingConcession> concessions
    )
    {
        return string.Join(", ", concessions.Select(x => x.Concession).Distinct().OrderBy(x => x));
    }
}
