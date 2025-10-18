using TaskSync.Domain.Project;
using TaskSync.Domain.User;

namespace TaskSync.Controllers.Response;

public class ProjectResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProjectVisibility? Visibility { get; set; }
    public User? ProjectManager { get; set; }
    public ICollection<ProjectMember> ProjectMembers { get; set; } = [];

    public User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }

    // Parameterless constructor for JSON deserialization (needed in Integration Tests)
    public ProjectResponse() 
    {
    }
    
    public ProjectResponse(Project project)
    {
        Id = project.Id;
        Title = project.Title;
        Description = project.Description;
        Visibility = project.Visibility;
        ProjectMembers = project.ProjectMembers;
        ProjectManager = project.ProjectMembers.FirstOrDefault(m => m.Role == "ProjectManager")?.User;
        CreatedBy = project.CreatedBy;
        CreatedDate = project.CreatedDate;
        ModifiedDate = project.ModifiedDate;
    }
}
