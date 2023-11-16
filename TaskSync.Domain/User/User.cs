namespace TaskSync.Domain.User;

public class User
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? Picture { get; set; }
}
