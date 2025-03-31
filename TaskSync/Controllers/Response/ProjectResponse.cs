using TaskSync.Domain.Project;
using TaskSync.Domain.User;

namespace TaskSync.Controllers.Response;

public class ProjectResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public ProjectVisibility? Visibility { get; set; }
    public User? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }

    public ProjectResponse(Project project)
    {
        Id = project.Id;
        Title = project.Title;
        Description = project.Description;
        CreatedBy = project.CreatedBy;
        CreatedDate = project.CreatedDate;
        ModifiedDate = project.ModifiedDate;
        Visibility = project.Visibility;
    }
}
