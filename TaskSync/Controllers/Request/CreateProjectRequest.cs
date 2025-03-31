using TaskSync.Domain.Project;
using TaskSync.Domain.Project.Commands;

namespace TaskSync.Controllers.Request;

public class CreateProjectRequest
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public ProjectVisibility? Visibility { get; set; } = ProjectVisibility.Everybody;

    public string? ProjectManagerId { get; set; }
    
    public CreateProjectCommand toCommand()
    {
        return new CreateProjectCommand
        {
            Description = Description, 
            Title = Title, 
            Visibility = Visibility, 
            ProjectManagerId = ProjectManagerId
        };
    }
}
