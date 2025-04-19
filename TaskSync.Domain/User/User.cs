namespace TaskSync.Domain.User;

public class User
{
    public int Id { get; set; } 
    public string? ExternalUserId { get; set; }
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? Picture { get; set; }
}
