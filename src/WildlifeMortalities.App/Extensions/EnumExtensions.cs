using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WildlifeMortalities.App.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue) =>
        enumValue == null
            ? string.Empty
            : enumValue.GetEnumValueCustomAttribute<DisplayAttribute>()?.GetName()
                ?? $"Error: enum {enumValue} is missing a displayname attribute";

    public static T? GetEnumValueCustomAttribute<T>(this Enum enumValue) where T : Attribute =>
        enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttributes<T>()
            .FirstOrDefault();
}
