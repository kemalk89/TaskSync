using Times.Domain.Shared;
using Times.Domain.Ticket;

namespace Times.Infrastructure.Entities;

public class TicketEntity : AuditedEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }

    public int ProjectId { get; set; }

    public ProjectEntity Project { get; set; } = null!;

    public Ticket ToTicket(User? createdBy = null)
    {
        return new Ticket
        {
            Id = Id,
            Title = Title,
            Description = Description,
            ProjectId = ProjectId,
            CreatedBy = createdBy,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate,
            Project = Project.ToProject()
        };
    }
}
