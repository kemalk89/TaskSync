using TaskSync.Domain.Shared;
using TaskSync.Domain.User.Command;

namespace TaskSync.Domain.User;

public interface IUserService
{
    Task<IEnumerable<User>> SearchUserAsync(SearchUserCommand searchUserCommand);

    Task<PagedResult<User>> GetUsersAsync(int pageNumber, int pageSize);
    
    Task<IEnumerable<User>> GetUsersAsync(int[] userIds);
}
