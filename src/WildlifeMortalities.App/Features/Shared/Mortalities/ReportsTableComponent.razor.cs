using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MudBlazor;
using WildlifeMortalities.Shared.Services.Reports;
using WildlifeMortalities.Shared.Services.Reports.QueryObjects;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class ReportsTableComponent : DbContextAwareComponent
{
    private const string Id = "Id";
    private const string Type = "Type";
    private const string Species = "Species";
    private const string Season = "Season";
    private const string DateSubmitted = "Date Submitted";
    private const string Status = "Status";
    public SortFilterPageOptions Options { get; set; } = new();

    [Parameter]
    public string? EnvClientId { get; set; }

    [Inject]
    private ReportService ReportService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private async Task<TableData<ReportListDto>> ServerReload(TableState state)
    {
        Options.OrderByAscending = state.SortDirection == SortDirection.Ascending;
        Options.OrderByOptions = state.SortLabel switch
        {
            Type => OrderByOptions.ByType,
            Season => OrderByOptions.BySeason,
            DateSubmitted => OrderByOptions.ByDateSubmitted,
            _ => OrderByOptions.SimpleOrder
        };

        var (reportDtos, totalItems) =
            EnvClientId == null
                ? await GetAllReports(state.Page + 1, state.PageSize)
                : await GetReportsByEnvClientId(EnvClientId, state.Page + 1, state.PageSize);
        return new TableData<ReportListDto> { Items = reportDtos, TotalItems = totalItems };
    }

    public void GotoDetails(TableRowClickEventArgs<ReportListDto> args)
    {
        NavigationManager.NavigateTo(
            Constants.Routes.GetReportDetailsPageLink(
                (args.Item.BadgeNumber ?? args.Item.EnvClientId)!,
                args.Item.Id
            )
        );
    }

    public async Task<(IEnumerable<ReportListDto>, int totalItems)> GetReports(
        string? envClientId,
        int pageNum = 0,
        int pageSize = 10
    )
    {
        Options.PageNumber = pageNum;
        Options.PageSize = pageSize;
        Options.EnvClientId = envClientId;

        var context = Context;
        var query = ReportService.SortFilterPage(Options, context);

        var preResult = await query;
        IEnumerable<ReportListDto> result =
            preResult is IAsyncQueryProvider
                ? await preResult.AsSplitQuery().ToArrayAsync()
                : preResult.AsSplitQuery().ToArray();

        return (result, Options.TotalItems);
    }

    public async Task<(IEnumerable<ReportListDto>, int totalItems)> GetAllReports(
        int pageNum = 0,
        int pageSize = 10
    )
    {
        return await GetReports(null, pageNum, pageSize);
    }

    public async Task<(IEnumerable<ReportListDto>, int totalItems)> GetReportsByEnvClientId(
        string envClientId,
        int pageNum = 0,
        int pageSize = 10
    )
    {
        return await GetReports(envClientId, pageNum, pageSize);
    }
}
