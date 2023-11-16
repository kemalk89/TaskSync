using TaskSync.Domain.Shared;
using TaskSync.Domain.User.Command;

namespace TaskSync.Domain.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> SearchUserAsync(SearchUserCommand searchUserCommand)
    {
        var users = await _userRepository.SearchUsersAsync(searchUserCommand.SearchText);
        return users;
    }

    public async Task<PagedResult<User>> GetUsersAsync(int pageNumber, int pageSize)
    {
        var users = await _userRepository.FindUsersAsync(pageNumber, pageSize);
        return new PagedResult<User>
        {
            Items = users,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

}
