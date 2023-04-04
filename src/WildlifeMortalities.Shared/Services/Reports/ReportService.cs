using WildlifeMortalities.Data;
using WildlifeMortalities.Shared.Services.Generic.QueryObjects;
using WildlifeMortalities.Shared.Services.Reports.QueryObjects;

namespace WildlifeMortalities.Shared.Services.Reports;

public class ReportService
{
    public static async Task<IQueryable<ReportDto>> SortFilterPage(
        SortFilterPageOptions options,
        AppDbContext context
    )
    {
        var query = await context.Reports
            .MapReportToDto()
            .OrderReportsBy(options.OrderByOptions, options.OrderByAscending)
            .FilterReportsBy(options.FilterBy, options.FilterValue);

        await options.SetupRestOfDto(query);
        return query.Page(options.PageNumber - 1, options.PageSize);
    }
}
