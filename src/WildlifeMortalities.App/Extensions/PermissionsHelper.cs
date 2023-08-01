using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.App.Extensions;

public class PermissionView
{
    public string Value { get; }
    public string Name { get; }

    public PermissionView(string name, string value)
    {
        Name = name;
        Value = value;
    }
}

public class PermissionGroup
{
    public PermissionGroup(string name, IEnumerable<PermissionView> permissions)
    {
        Name = name;
        Permissions = permissions;
    }

    public string Name { get; set; } = null!;
    public IEnumerable<PermissionView> Permissions { get; }
}

public static class PermissionsHelper
{
    public static IEnumerable<PermissionGroup> GetPermissions()
    {
        var type = typeof(PermissionConstants);

        List<PermissionGroup> result = new();
        foreach (var groupType in type.GetNestedTypes())
        {
            var name = groupType.Name;
            var members = groupType
                .GetFields()
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(
                    x =>
                        new PermissionView(
                            x.GetCustomAttribute<DisplayAttribute>()?.Name ?? string.Empty,
                            x.GetRawConstantValue()?.ToString() ?? string.Empty
                        )
                );

            result.Add(new PermissionGroup(name, members.ToArray()));
        }

        return result;
    }
}
