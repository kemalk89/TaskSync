namespace TaskSync.Domain.Sprint.AddSprint;

public class AddSprintCommand
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool IsActive { get; set; }
}