namespace WildlifeMortalities.Shared.Services.Reports.QueryObjects;

public enum FilterByOptions
{
    NoFilter = 0,
    ByEnvClientId,
    //ByType,
    //BySeason
}

public static class ReportDtoFilter
{
    public static IQueryable<ReportDto> FilterReportsBy(
        this IQueryable<ReportDto> reports,
        FilterByOptions filterByOptions,
        string filterValue
    )
    {
        if (string.IsNullOrEmpty(filterValue))
            return reports;

        return filterByOptions switch
        {
            FilterByOptions.NoFilter => reports,
            FilterByOptions.ByEnvClientId => reports.Where(x => x.EnvClientId == filterValue),
            _
                => throw new ArgumentOutOfRangeException(
                    nameof(filterByOptions),
                    filterByOptions,
                    null
                )
        };
    }
}
