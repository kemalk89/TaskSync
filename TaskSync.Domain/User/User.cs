namespace TaskSync.Domain.User;

public class User
{
    public int Id { get; set; }
    public string? ExternalUserId { get; set; }
    public string? Username { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Picture { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
    /// <summary>
    /// The selected language of the user. Example values: "de" or "en"
    /// </summary>
    public string? SelectedLanguage { get; set; }
}
