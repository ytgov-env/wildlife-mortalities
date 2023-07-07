using System.Reflection;

namespace WildlifeMortalities.Test.Helpers;

public static class TypeExtensions
{
    public static PropertyInfo[] GetPublicNoneBaseProperties(this Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

    public static PropertyInfo[] GetPublicBaseOnlyProperties(this Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Except(type.GetPublicNoneBaseProperties())
            .ToArray();

    public static IEnumerable<Type> GetAllDerivedTypes(this Type baseType)
    {
        return baseType.Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsSubclassOf(baseType))
            .ToArray();
    }

    public static IEnumerable<object[]> GetAllDerivedTypesExcludingSomeTypes(
        this Type baseType,
        Type[] excludedDerivedTypes
    )
    {
        return baseType
            .GetAllDerivedTypes()
            .Where(t => !excludedDerivedTypes.Contains(t))
            .Select(t => new object[] { t });
    }
}
