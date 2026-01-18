namespace TaskSync.Domain.Sprint;

public class SprintModel
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public bool IsActive { get; set; }
}