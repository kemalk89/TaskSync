namespace TaskSync.Domain.Project.Commands;

public class UpdateProjectCommand
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? ProjectManagerId { get; set; }
}