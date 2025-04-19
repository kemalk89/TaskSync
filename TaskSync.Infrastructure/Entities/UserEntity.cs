using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Entities;

public class UserEntity : AuditedEntity
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string Username { get; set; }
    public string? ExternalUserId { get; set; }
    public string? Picture { get; set; }

    public User ToUser()
    {
        return new User
        {
            Id = Id,
            Email = Email,
            Username = Username,
            Picture = Picture,
            ExternalUserId = ExternalUserId
        };
    }

    public void Update(User user)
    {
        Email = user.Email;
        Username = user.Username;
        Picture = user.Picture;
    }
}