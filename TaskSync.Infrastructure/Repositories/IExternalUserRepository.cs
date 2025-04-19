using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Repositories;

/// <summary>
/// Access user data from external data source.
/// </summary>
public interface IExternalUserRepository
{
    Task<User[]> SearchUsersAsync(string searchText);
    Task<User[]> FindUsersAsync(string[] userIds);
    Task<User?> FindUserByIdAsync(string userId);
    Task<User[]> FindUsersAsync(int pageNumber, int pageSize);
    Task<User?> FindUserByIdFromExternalSourceAsync(string externalUserId);

}