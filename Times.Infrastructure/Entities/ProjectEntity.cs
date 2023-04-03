using Times.Domain.Project;
using Times.Domain.User;

namespace Times.Infrastructure.Entities;

public class ProjectEntity : AuditedEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }

    public Project ToDomainObject(User? createdBy = null)
    {
        return new Project
        {
            Id = Id,
            Title = Title,
            Description = Description,
            CreatedBy = createdBy,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate
        };
    }
}
