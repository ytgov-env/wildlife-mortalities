using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.App.Features.Auth;

public class Selectable<T>
{
    public T Value { get; }
    public bool IsSelected { get; set; }

    public Selectable(T value, bool isSelected)
    {
        Value = value;
        IsSelected = isSelected;
    }

    public Selectable(T value)
        : this(value, false) { }
}

public class UserViewModel
{
    public User User { get; } = null!;
    public IEnumerable<Selectable<PermissionView>> Permissions { get; }

    public UserViewModel(User user, IEnumerable<PermissionView> possiblePermissions)
    {
        User = user;
        Permissions = possiblePermissions
            .Select(
                x =>
                    new Selectable<PermissionView>(x, user.Permissions.Any(y => y.Value == x.Value))
            )
            .ToList();
    }
}
