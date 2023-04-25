namespace WildlifeMortalities.Data.Entities.Users;

public class UserSettings
{
    public static UserSettings Default => new() { IsDarkMode = false };

    public bool IsDarkMode { get; set; }
}
