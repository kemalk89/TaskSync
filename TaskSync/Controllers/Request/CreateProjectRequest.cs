using TaskSync.Domain.Project;

namespace TaskSync.Controllers.Request;

public class CreateProjectRequest
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public ProjectVisibility? Visibility { get; set; } = ProjectVisibility.Everybody;

}
