using Times.Domain.Ticket;

namespace Times.Infrastructure.Entities;

public class TicketEntity : AuditedEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public int ProjectId { get; set; }

    public ProjectEntity Project { get; set; }

    public Ticket ToTicket()
    {
        return new Ticket
        {
            Id = Id,
            Title = Title,
            Description = Description,
            ProjectId = ProjectId,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate,
            Project = Project.ToProject()
        };
    }
}
