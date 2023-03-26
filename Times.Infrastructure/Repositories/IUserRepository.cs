using Times.Domain.Shared;

namespace Times.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User[]> FindUsersAsync(string[] userIds);
    Task<User> FindUserByIdAsync(string userId);
}
