using TaskSync.Domain.User.Command;

namespace TaskSync.Controllers.Request;

public class SearchUserRequest
{
    public string SearchText { get; set; } = null!;

    public SearchUserCommand ToCommand()
    {
        return new SearchUserCommand
        {
            SearchText = SearchText
        };
    }
}
