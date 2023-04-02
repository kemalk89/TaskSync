namespace Times.Domain.User;

public interface IUserRepository
{
    Task<User[]> SearchUsersAsync(string searchText);
    Task<User[]> FindUsersAsync(string[] userIds);
    Task<User?> FindUserByIdAsync(string userId);
}
