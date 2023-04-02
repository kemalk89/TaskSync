using Times.Domain.User;

namespace Times.Controllers.Response;

public class UserResponse
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public String Picture { get; set; }

    public UserResponse(User user)
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        Picture = user.Picture;
    }
}
