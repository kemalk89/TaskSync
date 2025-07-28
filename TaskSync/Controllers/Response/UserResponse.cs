using TaskSync.Domain.User;

namespace TaskSync.Controllers.Response;

public class UserResponse
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? Picture { get; set; }
    public string? ExternalSource { get; set; }

    public UserResponse(User user)
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        Picture = user.Picture;
        ExternalSource = user.ExternalUserId != null && user.ExternalUserId.StartsWith("auth0") ? "auth0" : null;
    }
}
