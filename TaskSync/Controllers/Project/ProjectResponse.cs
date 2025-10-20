using TaskSync.Domain.Project;
using TaskSync.Domain.User;

namespace TaskSync.Controllers.Project;

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
    
    public ProjectResponse(ProjectModel projectModel)
    {
        Id = projectModel.Id;
        Title = projectModel.Title;
        Description = projectModel.Description;
        Visibility = projectModel.Visibility;
        ProjectMembers = projectModel.ProjectMembers;
        ProjectManager = projectModel.ProjectMembers.FirstOrDefault(m => m.Role == "ProjectManager")?.User;
        CreatedBy = projectModel.CreatedBy;
        CreatedDate = projectModel.CreatedDate;
        ModifiedDate = projectModel.ModifiedDate;
    }
}
