using System.Linq.Expressions;

namespace WildlifeMortalities.Shared.Services.Reports.QueryObjects;

public enum OrderByOptions
{
    SimpleOrder = 0,
    ByType,

    //BySpecies,
    BySeason,
    ByDateSubmitted,
}

public static class ReportListDtoSort
{
    public static IQueryable<ReportListDto> OrderReportsBy(
        this IQueryable<ReportListDto> reports,
        OrderByOptions orderByOptions,
        bool ascending
    )
    {
        Expression<Func<ReportListDto, object>> method = orderByOptions switch
        {
            OrderByOptions.SimpleOrder => x => x.Id,
            OrderByOptions.ByType => x => x.Type,
            OrderByOptions.BySeason => x => x.Season.StartDate,
            OrderByOptions.ByDateSubmitted => x => x.DateSubmitted,
            _ => throw new ArgumentOutOfRangeException(nameof(orderByOptions), orderByOptions, null)
        };

        return ascending ? reports.OrderBy(method) : reports.OrderByDescending(method);
    }
}
