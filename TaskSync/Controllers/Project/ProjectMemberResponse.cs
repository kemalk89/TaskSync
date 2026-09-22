using TaskSync.Domain.Project;
using TaskSync.Domain.User;

namespace TaskSync.Controllers.Project;

public class ProjectMemberResponse
{
    public int UserId { get; set; }
    public string Role { get; set; } = string.Empty;
    public User? User { get; set; }

    // Parameterless constructor for JSON deserialization (needed in Integration Tests)
    public ProjectMemberResponse()
    {
    }

    public ProjectMemberResponse(ProjectMemberModel model)
    {
        UserId = model.UserId;
        Role = model.Role;
        User = model.User;
    }
}
