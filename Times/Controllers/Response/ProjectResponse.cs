using Times.Domain.Project;

namespace Times.Controllers.Response;

public class ProjectResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string CreatedBy { get; set; }
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
    }
}
