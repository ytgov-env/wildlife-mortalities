using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MudBlazor;
using WildlifeMortalities.Data;
using WildlifeMortalities.Shared.Services.Reports;
using WildlifeMortalities.Shared.Services.Reports.QueryObjects;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class MortalityReportsTableComponent : DbContextAwareComponent
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
    private IDbContextFactory<AppDbContext> DbContextFactory { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private async Task<TableData<ReportDto>> ServerReload(TableState state)
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
        return new TableData<ReportDto> { Items = reportDtos, TotalItems = totalItems };
    }

    private void CreateMortalityReport()
    {
        NavigationManager.NavigateTo($"mortality-reports/new/{EnvClientId}");
    }

    public void GotoDetails(TableRowClickEventArgs<ReportDto> args)
    {
        NavigationManager.NavigateTo($"mortality-reports/{args.Item.Id}");
    }

    public async Task<(IEnumerable<ReportDto>, int totalItems)> GetReports(
        string? envClientId,
        int pageNum = 0,
        int pageSize = 10
    )
    {
        Options.PageNumber = pageNum;
        Options.PageSize = pageSize;
        if (envClientId != null)
        {
            Options.FilterBy = FilterByOptions.ByEnvClientId;
            Options.FilterValue = envClientId;
        }

        var context = DbContextFactory.CreateDbContext();
        var query = ReportService.SortFilterPage(Options, context);

        var preResult = await query;
        IEnumerable<ReportDto> result =
            preResult is IAsyncQueryProvider
                ? await preResult.AsSplitQuery().ToArrayAsync()
                : preResult.ToArray();

        return (result, Options.TotalItems);
    }

    public async Task<(IEnumerable<ReportDto>, int totalItems)> GetAllReports(
        int pageNum = 0,
        int pageSize = 10
    )
    {
        return await GetReports(null, pageNum, pageSize);
    }

    public async Task<(IEnumerable<ReportDto>, int totalItems)> GetReportsByEnvClientId(
        string envClientId,
        int pageNum = 0,
        int pageSize = 10
    )
    {
        return await GetReports(envClientId, pageNum, pageSize);
    }
}
