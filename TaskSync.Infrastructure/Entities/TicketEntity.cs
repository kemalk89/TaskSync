using TaskSync.Domain.Ticket;
using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Entities;

public class TicketEntity : AuditedEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }

    public int ProjectId { get; set; }
    public int? StatusId { get; set; }

    public TicketStatusEntity? Status { get; set; }

    public ProjectEntity Project { get; set; } = null!;

    public string? AssigneeId { get; set; }

    public TicketModel ToTicket(User? createdBy = null, User? assignee = null)
    {
        return new TicketModel
        {
            Id = Id,
            Title = Title,
            Description = Description,
            ProjectId = ProjectId,
            Assignee = assignee,
            CreatedBy = createdBy,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate,
            Status = Status?.ToDomainObject(),
            Project = Project.ToDomainObject()
        };
    }

    public bool HasAssignee()
    {
        return AssigneeId != null;
    }
}
