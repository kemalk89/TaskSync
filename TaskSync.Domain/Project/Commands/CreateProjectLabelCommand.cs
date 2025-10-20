namespace TaskSync.Domain.Project.Commands;

public class CreateProjectLabelCommand
{
    public int ProjectId { get; set; }
    public string Text { get; set; } = string.Empty;
}