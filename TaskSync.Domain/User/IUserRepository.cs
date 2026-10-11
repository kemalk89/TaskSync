using TaskSync.Domain.Shared;

namespace TaskSync.Domain.User;

public interface IUserRepository
{
    Task<User[]> SearchUsersAsync(string searchText);
    Task<User[]> FindUsersAsync(int[] userIds);
    Task<User?> FindUserByIdAsync(int userId);
    Task<User?> FindUserByEmailAsync(string email);
    Task<User?> FindUserByExternalUserIdAsync(string externalUserId);
    Task<User[]> FindUsersAsync(int pageNumber, int pageSize);
    Task<int> SaveNewExternalUserAsync(User user);
    Task<Result<int>> SaveNewUserAsync(User user, string password);
    Task<int> UpdateUserAsync(User user);
    Task<Result<bool>> VerifyPasswordAsync(string email, string passwordHash, string password);
}
