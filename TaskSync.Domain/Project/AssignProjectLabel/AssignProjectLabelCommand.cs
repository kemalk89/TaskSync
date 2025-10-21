namespace TaskSync.Domain.Project.AssignProjectLabel;

public class AssignProjectLabelCommand
{
    public int ProjectId { get; set; }
    public string Text { get; set; } = string.Empty;
}