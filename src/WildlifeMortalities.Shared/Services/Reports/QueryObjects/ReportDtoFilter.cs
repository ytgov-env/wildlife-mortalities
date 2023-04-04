using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Shared.Services.Reports.QueryObjects;

public enum FilterByOptions
{
    NoFilter = 0,
    ByEnvClientId,
    BySpecies,

    ByType,
    BySeason
}

public static class ReportDtoFilter
{
    public static async Task<IQueryable<ReportDto>> FilterReportsBy(
        this IQueryable<ReportDto> reports,
        FilterByOptions filterByOptions,
        string filterValue
    )
    {
        if (string.IsNullOrEmpty(filterValue))
            return reports;
        //filterByOptions = FilterByOptions.BySpecies;
        //filterValue = "GrizzlyBear";

        return filterByOptions switch
        {
            FilterByOptions.NoFilter => reports,
            FilterByOptions.ByEnvClientId => reports.Where(x => x.EnvClientId == filterValue),
            FilterByOptions.BySpecies
                => (await reports.ToListAsync())
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
