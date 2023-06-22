namespace WildlifeMortalities.Test.Helpers;

internal static class ThreadSafeRandom
{
    private static readonly Random s_globalRandom = new();

    [ThreadStatic]
    private static Random? s_localRandom;

    public static int Next()
    {
        if (s_localRandom == null)
        {
            var seed = 0;
            lock (s_globalRandom)
            {
                seed = s_globalRandom.Next();
            }

            s_localRandom = new Random(seed);
        }

        return s_localRandom.Next();
    }
}
