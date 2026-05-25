using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Entities;

public class UserEntity : AuditedEntity
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? ExternalUserId { get; set; }
    public string? Picture { get; set; }
    /// <summary>
    /// The selected language of the user. Example values: "de" or "en"
    /// </summary>
    public string? SelectedLanguage { get; set; }

    public User ToUser()
    {
        return new User
        {
            Id = Id,
            Email = Email,
            Username = Username,
            Picture = Picture,
            ExternalUserId = ExternalUserId,
            SelectedLanguage = SelectedLanguage,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate
        };
    }

    public void Update(User user)
    {
        Email = user.Email;
        Username = user.Username;
        Picture = user.Picture;
        ExternalUserId = user.ExternalUserId;
        SelectedLanguage = user.SelectedLanguage;
        ModifiedDate = DateTimeOffset.Now;
    }
}