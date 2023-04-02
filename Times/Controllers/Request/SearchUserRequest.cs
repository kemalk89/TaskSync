using Times.Domain.User.Command;

namespace Times.Controllers.Request;

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
