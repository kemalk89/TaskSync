using TaskSync.Domain.Shared;

namespace TaskSync.Domain.User;

public interface IUserRepository
{
    Task<User[]> SearchUsersAsync(string searchText);
    Task<User[]> FindUsersAsync(string[] userIds);
    Task<User?> FindUserByIdAsync(string userId);
    Task<User?> FindUserByEmailAsync(string email);
    Task<User?> FindUserByExternalUserIdAsync(string externalUserId);
    Task<User[]> FindUsersAsync(int pageNumber, int pageSize);
    Task<string> SaveNewExternalUserAsync(User user);
    Task<Result<string>> SaveNewUserAsync(User user, string password);
    Task<string> UpdateUserAsync(User user);
    Task<Result<bool>> VerifyPasswordAsync(string email, string passwordHash, string password);
}
