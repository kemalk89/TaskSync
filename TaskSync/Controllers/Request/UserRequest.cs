using System.ComponentModel.DataAnnotations;
using TaskSync.Domain.User;

namespace TaskSync.Controllers.Request;

public class UserRequest
{
    [Required]
    public string Id { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? Picture { get; set; }

    public User ToUser()
    {
        return new User
        {
            Id = Id,
            Username = Username,
            Email = Email,
            Picture = Picture
        };
    }
}
