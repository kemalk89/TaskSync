namespace TaskSync.Domain.Project;

public class ProjectMemberModel
{
    public string Role { get; set; } = string.Empty;
    public required int UserId { get; set; }
    public User.User? User { get; set; }
}