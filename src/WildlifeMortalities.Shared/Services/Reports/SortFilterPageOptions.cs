using WildlifeMortalities.Shared.Services.Reports.QueryObjects;

namespace WildlifeMortalities.Shared.Services.Reports;

public class SortFilterPageOptions
{
    public const int DefaultPageSize = 10;

    public int[] _pageSizes = new[] { 5, DefaultPageSize, 20, 50, 100, 500, 1000 };

    public OrderByOptions OrderByOptions { get; set; }

    public FilterByOptions FilterBy { get; set; }

    public string FilterValue { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = DefaultPageSize;

    public int NumberOfPages { get; private set; }

    public int TotalItems { get; private set; }

    public string PrevCheckState { get; set; }

    public bool OrderByAscending { get; set; } = true;

    public void SetupRestOfDto<T>(IQueryable<T> query)
    {
        TotalItems = query.Count();
        NumberOfPages = (int)Math.Ceiling((double)TotalItems / PageSize);
        PageNumber = Math.Min(Math.Max(1, PageNumber), NumberOfPages);

        var newCheckState = GenerateCheckState();
        if (PrevCheckState != newCheckState)
            PageNumber = 1;

        PrevCheckState = newCheckState;
    }

    private string GenerateCheckState()
    {
        return $"{(int)FilterBy},{FilterValue},{PageSize},{NumberOfPages}";
    }
}
