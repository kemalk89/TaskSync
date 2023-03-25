using Times.Domain.Project;

namespace Times.Infrastructure.Entities;

public class ProjectEntity : AuditedEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public Project ToProject()
    {
        return new Project
        {
            Id = Id,
            Title = Title,
            Description = Description,
            CreatedBy = CreatedBy,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate
        };
    }
}
