using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Shared.Services.Reports.QueryObjects;

public enum FilterByOptions
{
    NoFilter = 0,
    BySpecies,
    ByType,
    BySeason
}

public static class ReportDtoFilter
{
    public static async Task<IQueryable<ReportDto>> FilterReportsBy(
        this IQueryable<ReportDto> reports,
        FilterByOptions filterByOptions,
        string filterValue,
        string? envClientId
    )
    {
        if (string.IsNullOrEmpty(envClientId) == false)
        {
            reports = reports.Where(x => x.EnvClientId == envClientId);
        }

        if (string.IsNullOrEmpty(filterValue))
            return reports;
        //filterByOptions = FilterByOptions.BySpecies;
        //filterValue = "GrizzlyBear";

        return filterByOptions switch
        {
            FilterByOptions.NoFilter => reports,
            // It's not possible for EF Core to translate the species filter to SQL, so we must perform the sequence filtering in memory,
            // and then transform the sequence back into an IQueryable for subsequent operations
            FilterByOptions.BySpecies
                => (await reports.ToArrayAsync())
                    .Where(x => x.SpeciesCollection.Contains(Enum.Parse<Species>(filterValue)))
                    .AsQueryable(),
            FilterByOptions.ByType => reports.Where(x => x.Type == filterValue),
            _
                => throw new ArgumentOutOfRangeException(
                    nameof(filterByOptions),
                    filterByOptions,
                    null
                )
        };
    }
}
