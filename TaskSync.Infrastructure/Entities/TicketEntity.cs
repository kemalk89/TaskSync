using TaskSync.Domain.Ticket;
using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Entities;

public class TicketEntity : AuditedEntity
{
    public int Id { get; set; }
    public int Position { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }

    public TicketType Type { get; set; }

    public int ProjectId { get; set; }
    public int? StatusId { get; set; }
    
    public int? SprintId { get; set; }

    public TicketStatusEntity? Status { get; set; }

    public ProjectEntity? Project { get; set; }

    public int? AssigneeId { get; set; }

    public ICollection<TicketLabelEntity> Labels { get; set; } = [];
    
    public TicketModel ToTicket(User? createdBy = null, User? assignee = null)
    {
        return new TicketModel
        {
            Id = Id,
            Position = Position,
            Title = Title,
            Description = Description,
            ProjectId = ProjectId,
            Assignee = assignee,
            CreatedBy = createdBy,
            CreatedDate = CreatedDate,
            ModifiedDate = ModifiedDate,
            Status = Status?.ToDomainObject(),
            ProjectModel = Project?.ToDomainObject(),
            Labels = Labels.Select(i => new TicketLabelModel{Id = i.Id, Text = i.Text}).ToList(),
            Type = Type
        };
    }
}


