using Times.Domain.Project;
using Times.Domain.Ticket;

namespace Times.Infrastructure.Entities;

public class ProjectEntity : AuditedEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public Ticket ToTicket()
    {
        return new Ticket
        {
            Id = Id,
            Title = Title,
            Description = Description
        };
    }

    public Project ToProject()
    {
        return new Project
        {
            Id = Id,
            Title = Title,
            Description = Description
        };
    }
}
