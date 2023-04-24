namespace WildlifeMortalities.Shared.Services.Generic.QueryObjects;

public static class GenericPaging
{
    public static IQueryable<T> Page<T>(
        this IQueryable<T> query,
        int pageNumberZeroStart,
        int pageSize
    )
    {
        if (pageSize == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "pageSize cannot be zero.");
        }
        if (pageNumberZeroStart > 0)
        {
            query = query.Skip(pageNumberZeroStart * pageSize);
        }
        return query.Take(pageSize);
    }
}
