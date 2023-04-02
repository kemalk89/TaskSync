using Times.Domain.User.Command;

namespace Times.Domain.User;

public interface IUserService
{
    Task<IEnumerable<User>> SearchUserAsync(SearchUserCommand searchUserCommand);
}
