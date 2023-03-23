namespace WildlifeMortalities.App.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> ValueOrEmpty<T>(this IEnumerable<T>? sequence)
    {
        return sequence ?? Array.Empty<T>();
    }
}
