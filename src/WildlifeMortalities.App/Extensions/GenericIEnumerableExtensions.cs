namespace WildlifeMortalities.App.Extensions;

public static class GenericIEnumerableExtensions
{
    public static IEnumerable<T> ValueOrEmpty<T>(this IEnumerable<T>? sequence)
    {
        return sequence ?? Array.Empty<T>();
    }
}
