namespace WildlifeMortalities.Data.Extensions;

internal static class SpeciesExtensions
{
    public static bool IsBigGameSpecies(this Species enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsBigGameAttribute>() != null;
}
