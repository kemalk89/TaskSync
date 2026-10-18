namespace TaskSync.Domain.Project;

public class ProjectMemberModel
{
    public string Role { get; set; } = string.Empty;
    public required string UserId { get; set; }
    public User.User? User { get; set; }
}