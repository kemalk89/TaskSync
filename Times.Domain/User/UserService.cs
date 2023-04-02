using Times.Domain.User.Command;

namespace Times.Domain.User;

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
}
