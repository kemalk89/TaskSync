using TaskSync.Domain.Sprint;

namespace TaskSync.Infrastructure.Entities;

public class SprintEntity : AuditedEntity
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool IsActive { get; set; }
    
    public SprintModel ToModel()
    {
        return new SprintModel
        {
            Id = Id,
            ProjectId = ProjectId, 
            Name = Name, 
            StartDate = StartDate, 
            EndDate = EndDate, 
            IsActive = IsActive
        };
    }
}