using Microsoft.AspNetCore.Identity;

using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Entities;

public class UserEntity : IdentityUser, IAuditedEntity
{
    public string? ExternalUserId { get; set; }
    public string? Picture { get; set; }
    /// <summary>
    /// The selected language of the user. Example values: "de" or "en"
    /// </summary>
    public string? SelectedLanguage { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }

    public User ToUser()
    {
        return new User
        {
            Id = Id,
            Email = Email ?? string.Empty, 
            Username = UserName,
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
        UserName = user.Username;
        Picture = user.Picture;
        ExternalUserId = user.ExternalUserId;
        SelectedLanguage = user.SelectedLanguage;
        ModifiedDate = DateTimeOffset.Now;
    }
}