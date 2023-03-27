using WildlifeMortalities.Data;
using WildlifeMortalities.Shared.Services.Generic.QueryObjects;
using WildlifeMortalities.Shared.Services.Reports.QueryObjects;

namespace WildlifeMortalities.Shared.Services.Reports;

public class ReportService
{
    public static IQueryable<ReportDto> SortFilterPage(
        SortFilterPageOptions options,
        AppDbContext context
    )
    {
        var query = context.Reports
            .MapReportToDto()
            .OrderReportsBy(options.OrderByOptions)
            .FilterReportsBy(options.FilterBy, options.FilterValue);

        options.SetupRestOfDto(query);
        return query.Page(options.PageNumber - 1, options.PageSize);
    }
}
