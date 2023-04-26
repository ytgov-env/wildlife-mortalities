namespace WildlifeMortalities.Shared.Services;

public interface IAppConfigurationService
{
    Task<T> GetValue<T>(string key);
    Task SetValue<T>(string key, T value);
    Task<(bool, T?)> TryGetValue<T>(string key);
    Task<T> TryGetValue<T>(string key, T fallbackValue);
}
