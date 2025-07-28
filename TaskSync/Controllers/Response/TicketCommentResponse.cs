using TaskSync.Domain.Ticket;

namespace TaskSync.Controllers.Response;

public class TicketCommentResponse
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public string Comment { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public UserResponse? CreatedBy { get; set; }
    
    public DateTimeOffset CreatedDate { get; set; }
    
    public TicketCommentResponse(TicketCommentModel model)
    {
        Id = model.Id;
        TicketId = model.TicketId;
        Comment = model.Comment;
        IsDeleted = model.IsDeleted;
        CreatedBy = model.CreatedBy == null ? null : new UserResponse(model.CreatedBy);
        CreatedDate = model.CreatedDate;
    }
}