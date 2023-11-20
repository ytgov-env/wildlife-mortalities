using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.Shared.Services;

public interface IUserAuthorizationService
{
    Task<bool> CreateUser(string email);
    Task<bool> DeleteUser(int userId);
    Task UpdatePermission(int userId, string permission, bool hasPermission);
}
