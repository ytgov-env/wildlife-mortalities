namespace WildlifeMortalities.Data.Extensions;

public static class SpeciesExtensions
{
    public static bool IsBigGameSpecies(this Species enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsBigGameAttribute>() != null;
}
