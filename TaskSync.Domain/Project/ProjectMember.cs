namespace TaskSync.Domain.Project;

public class ProjectMember
{
    public string Role { get; set; } = string.Empty;
    public User.User User { get; set; }
}