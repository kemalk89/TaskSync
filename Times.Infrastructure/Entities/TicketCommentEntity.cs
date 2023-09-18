using Times.Domain.Ticket;
using Times.Domain.User;

namespace Times.Infrastructure.Entities;

public class TicketCommentEntity : AuditedEntity
{
    public int Id { get; set; }

    public int TicketId { get; set; }
    
    public TicketEntity Ticket { get; set; }
    
    public string Comment { get; set; }

    public TicketCommentModel ToModel(User? createdBy = null)
    {
        return new TicketCommentModel
        {
            Id = Id,
            TicketId = TicketId,
            Comment = Comment,
            CreatedBy = createdBy,
            CreatedDate = CreatedDate
        };
    }
}