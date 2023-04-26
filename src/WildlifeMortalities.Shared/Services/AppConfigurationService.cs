using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.Shared.Services;

public class AppConfigurationService : IAppConfigurationService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public AppConfigurationService(IDbContextFactory<AppDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public async Task<(bool, T?)> TryGetValue<T>(string key)
    {
        try
        {
            var value = await GetValue<T>(key);
            return (true, value);
        }
        catch (Exception)
        {
            return (false, default);
        }
    }

    public async Task<T> TryGetValue<T>(string key, T fallbackValue)
    {
        var (found, value) = await TryGetValue<T>(key);
        return found ? value! : fallbackValue;
    }

    public async Task<T> GetValue<T>(string key)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var appConfiguration =
            await context.AppConfiguration.FirstOrDefaultAsync(x => x.Key == key)
            ?? throw new ArgumentException("Key not found");
        if (appConfiguration.Value != null)
        {
            var result = JsonSerializer.Deserialize<T>(appConfiguration.Value);
            return result == null
                ? throw new ArgumentException(
                    $"Cannot convert value {appConfiguration.Value} to type {typeof(T).Name}"
                )
                : result;
        }
        throw new ArgumentException($"Value for key {appConfiguration.Key} is null");
    }

    public async Task SetValue<T>(string key, T value)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var appConfiguration = await context.AppConfiguration.FirstOrDefaultAsync(
            x => x.Key == key
        );
        if (appConfiguration == null)
        {
            appConfiguration = new() { Key = key, };
            context.Add(appConfiguration);
        }

        appConfiguration.Value = JsonSerializer.Serialize(value);
        await context.SaveChangesAsync();
    }
}
