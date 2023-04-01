namespace WildlifeMortalities.Data.Entities.Users;

public class UserSettings
{
    public static UserSettings Default => new() { IsLightMode = false };

    public bool IsLightMode { get; set; }
}
