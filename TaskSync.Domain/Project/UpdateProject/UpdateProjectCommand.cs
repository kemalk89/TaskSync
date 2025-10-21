namespace TaskSync.Domain.Project.UpdateProject;

public class UpdateProjectCommand
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? ProjectManagerId { get; set; }
}