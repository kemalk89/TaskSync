namespace TaskSync.Domain.Project.Commands;

public class CreateProjectCommand
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ProjectVisibility? Visibility { get; set; } = ProjectVisibility.Everybody;
    
    public int? ProjectManagerId { get; set; }
}
