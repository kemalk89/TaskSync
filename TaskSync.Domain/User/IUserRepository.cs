namespace TaskSync.Domain.User;

public interface IUserRepository
{
    Task<User[]> SearchUsersAsync(string searchText);
    Task<User[]> FindUsersAsync(int[] userIds);
    Task<User?> FindUserByIdAsync(int userId);
    Task<User?> FindUserByEmailAsync(string email);
    Task<User?> FindUserByExternalUserIdAsync(string externalUserId);
    Task<User[]> FindUsersAsync(int pageNumber, int pageSize);
    Task<int> SaveNewUserAsync(User user);
    Task<int> UpdateUserAsync(User user);
}
