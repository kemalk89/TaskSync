namespace TaskSync.Domain.Project;

public class ProjectMember
{
    public string Role { get; set; }
    public required int UserId { get; set; }
    public User.User? User { get; set; }
}