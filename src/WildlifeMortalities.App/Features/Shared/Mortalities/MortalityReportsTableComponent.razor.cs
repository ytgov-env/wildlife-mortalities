using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using WildlifeMortalities.Data;
using WildlifeMortalities.Shared.Services.Reports;
using WildlifeMortalities.Shared.Services.Reports.QueryObjects;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class MortalityReportsTableComponent : DbContextAwareComponent
{
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
        var sortDirection = state.SortDirection;
        switch (state.SortLabel)
        {
            case "type_field":
                if (sortDirection == SortDirection.Ascending)
                {
                    Options.OrderByOptions = OrderByOptions.ByTypeAscending;
                    break;
                }
                else if (sortDirection == SortDirection.Descending)
                {
                    Options.OrderByOptions = OrderByOptions.ByTypeDescending;
                    break;
                }
                else
                {
                    Options.OrderByOptions = OrderByOptions.SimpleOrder;
                    break;
                }
            case "date_submitted_field":
                if (sortDirection == SortDirection.Ascending)
                {
                    Options.OrderByOptions = OrderByOptions.ByDateSubmittedAscending;
                    break;
                }
                else if (sortDirection == SortDirection.Descending)
                {
                    Options.OrderByOptions = OrderByOptions.ByDateSubmittedDescending;
                    break;
                }
                else
                {
                    Options.OrderByOptions = OrderByOptions.SimpleOrder;
                    break;
                }
        }
        var (reportDtos, totalItems) =
            EnvClientId == null
                ? await GetAllReports(state.Page, state.PageSize)
                : await GetReportsByEnvClientId(EnvClientId, state.Page, state.PageSize);
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

        return (await query.AsSplitQuery().ToArrayAsync(), Options.TotalItems);
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
