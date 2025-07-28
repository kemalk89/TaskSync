using TaskSync.Domain.Ticket;
using TaskSync.Domain.User;

namespace TaskSync.Infrastructure.Entities;

public class TicketCommentEntity : AuditedEntity
{
    public int Id { get; set; }

    public int TicketId { get; set; }
    
    public TicketEntity Ticket { get; set; }
    
    public string Comment { get; set; }

    public bool IsDeleted { get; set; }
    
    public TicketCommentModel ToModel(User? createdBy = null)
    {
        return new TicketCommentModel
        {
            Id = Id,
            TicketId = TicketId,
            Comment = Comment,
            IsDeleted = IsDeleted,
            CreatedBy = createdBy,
            CreatedById = CreatedBy,
            CreatedDate = CreatedDate
        };
    }
}