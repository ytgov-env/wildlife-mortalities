namespace WildlifeMortalities.Shared.Services.Reports.QueryObjects;

public enum OrderByOptions
{
    SimpleOrder = 0,
    ByDateSubmittedAscending,
    ByDateSubmittedDescending,
    ByTypeAscending,
    ByTypeDescending
}

public static class ReportDtoSort
{
    public static IQueryable<ReportDto> OrderReportsBy(
        this IQueryable<ReportDto> reports,
        OrderByOptions orderByOptions
    )
    {
        return orderByOptions switch
        {
            OrderByOptions.SimpleOrder => reports.OrderByDescending(x => x.Id),
            OrderByOptions.ByDateSubmittedAscending => reports.OrderBy(x => x.DateSubmitted),
            OrderByOptions.ByDateSubmittedDescending
                => reports.OrderByDescending(x => x.DateSubmitted),
            OrderByOptions.ByTypeAscending => reports.OrderBy(x => x.Type),
            OrderByOptions.ByTypeDescending => reports.OrderByDescending(x => x.Type),
            _ => throw new ArgumentOutOfRangeException(nameof(orderByOptions), orderByOptions, null)
        };
    }
}
